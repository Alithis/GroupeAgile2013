using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Configuration;
using System.Web.Configuration;
using System.Collections.Concurrent;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace NoteTaLoc.Utilitary
{

    public class CategoryStorage
    {

        // Chemin vers le dossier contenant les fichiers de parametrage
        private String folderLocation;
        // Liste des categories
        private ConcurrentDictionary<String, Categories> categoriesList;
        // Liste des criteres
        private ConcurrentDictionary<String, ConcurrentDictionary<String,Criteria>> criteriaList;
        // utilitaires
        private TwitterError twitterError;
        private readonly Configuration config;
        public StringWriter errorString;
        // constantes
        public static readonly String SEPARATOR = "_lg_";
        public static readonly String XML_FILE_EXTENSION = "xml";
        public static readonly String DEFAULT_LANGUAGE = "eng";

        // Le constructeur charge le fichier de parametrage au demarrage
        public CategoryStorage(TwitterError tmpTwittError, Configuration tmpConf)
        {

            // allocation des outils
            twitterError = tmpTwittError;
            config = tmpConf;
            folderLocation = config.AppSettings.Settings["CategoryFolderLocation"].Value;

            // gestion des erreurs
            errorString = new StringWriter();

            // Chargement des criteres dans une liste, facilite l'acces aux donnees
            criteriaList = new ConcurrentDictionary<String, ConcurrentDictionary<String, Criteria>>();
            categoriesList = new ConcurrentDictionary<string, Categories>();

            // chargement du fichier de configuration
            try
            {
                // test du folder
                DirectoryInfo folder = new DirectoryInfo(folderLocation);

                // parcours de tous les fichiers dans le repertoire pour recuperer les langues
                foreach (FileInfo tmpFile in folder.EnumerateFiles())
                {
                    try
                    {
                        // si les fichiers ne sont pas null ou vide on traite
                        if (tmpFile != null && tmpFile.Name != null && !XML_FILE_EXTENSION.Equals(tmpFile.Extension.ToLower()))
                        {

                            // recuperation de la langue, le fichier doit etre nomme langue_lg_NomDuFichier.xml
                            int token = tmpFile.Name.LastIndexOf(CategoryStorage.SEPARATOR);
                            String language;
                            if (token > 1)
                            {
                                // usine a gaz pour recuperer la langue
                                language = tmpFile.Name.Substring(0,token).ToLower();
                            }
                            else
                            {
                                // si le token n'existe pas on passe au fichier suivant
                                continue;
                            }

                            // si une langue existe on poursuit le traitement
                            if (!String.Empty.Equals(language.Trim()))
                            {
                                // on recupere la liste de categorie dans le fichier xml
                                Categories tmpCategories = loadCategory(tmpFile.FullName);
                                // on ajoute chaque nouvelle categorie a la liste pour faciliter la recherche par langue
                                categoriesList.GetOrAdd(language, tmpCategories);

                                ConcurrentDictionary<String, Criteria> tmpCritList = new ConcurrentDictionary<string, Criteria>();

                                // on parcours la liste des categories
                                foreach (Category tmpCat in tmpCategories.categoriesList)
                                {
                                    // pour chaque categorie ou parcours la liste des criteres
                                    foreach (Criteria tmpCrit in tmpCat.criteriasList)
                                    {
                                        // on met chaque critere dans la liste avec l'id en cle pour faciliter la recherche par critere
                                        tmpCritList.GetOrAdd(tmpCrit.criteriaId.ToString(), tmpCrit);
                                    }
                                }
                                // on ajoute les criteres de la categorie avec la langue pour faciliter la recherche
                                criteriaList.GetOrAdd(language, tmpCritList);
                            }
                            else
                            {
                                errorString.Write("Pas de langue pour le fichier: " + tmpFile.Name + " ");
                            }
                        }
                        else
                        {
                            errorString.Write("Une erreur s'est produite en traitant le fichier: " + tmpFile.Name + " ");
                        }
                    }
                    catch (Exception EXP)
                    {
                        //TODO TRAITER L'EXCEPTION ICI access aux fichiers
                        defaultParametering();
                    }
                }

            }
            catch (Exception EXP)
            {
                //TODO TRAITER L'EXCEPTION ICI access au repertoire
                defaultParametering();
            }

            // Si les listes sont vides et/ou qu'un message d'erreur est present on charge des valeurs par defaut
            if (!errorString.ToString().Equals(String.Empty) || categoriesList.IsEmpty || criteriaList.IsEmpty)
            {
                defaultParametering();
            }

        }

        private void defaultParametering()
        {
            twitterError.publishError("INIT CATEGORIES NOK DEFAULT MODE ON");
            Categories cats = new Categories();
            cats.language = "eng";
            Category cat = new Category();
            cat.active = true;
            cat.categoryId = 10;
            cat.libelle = "libelle test";
            Category cat2 = new Category();
            cat2.active = true;
            cat2.categoryId = 20;
            cat2.libelle = "libelle test 2";
            Criteria crit1 = new Criteria();
            crit1.active = true;
            crit1.criteriaId = 10;
            crit1.libelle = "criteria libelle 1";
            crit1.weight = 1;
            Criteria crit2 = new Criteria();
            crit2.active = true;
            crit2.criteriaId = 10;
            crit2.libelle = "criteria libelle 2";
            crit2.weight = 1;
            cat.criteriasList = new List<Criteria>();
            cat.criteriasList.Add(crit1);
            cat.criteriasList.Add(crit2);
            cat2.criteriasList = new List<Criteria>();
            cat2.criteriasList.Add(crit1);
            cat2.criteriasList.Add(crit2);
            cats.categoriesList = new List<Category>();
            cats.categoriesList.Add(cat);
            cats.categoriesList.Add(cat2);
            ConcurrentDictionary<string, Categories> defaultCat = new ConcurrentDictionary<string, Categories>();
            defaultCat.GetOrAdd(DEFAULT_LANGUAGE, cats);
            this.categoriesList = defaultCat;
        }

        // Summary:
        //      Chargement de la categorie depuis le fichier xml
        private Categories loadCategory(String fileLocation)
        {
            //implementation de la methode avec un fichier xml et un serialisateur
            XmlSerializer serializer = new XmlSerializer(typeof(Categories));
            TextReader tr = new StreamReader(fileLocation);
            Categories tmpCat = (Categories)serializer.Deserialize(tr);
            tr.Close();
            return tmpCat;
        }

        // Summary:
        //      Cette method renvoie une categorie donnee pour un id de cat et une langue
        // Parameters:
        //      catId = id de la categorie, a recuperer dans la table criteria
        //      langue = langue en cours sur le site, a recuperer en session
        // Returns:
        //     la categorie
        public Category getCategoryLangId(String langue, int catId)
        {
            // teste la validite des parametres
            if (langue == null || String.Empty.Equals(langue.Trim()))
                throw new NullReferenceException("Erreur dans l'appel de la methode CategoryStorage.getCategory, le parametre langue est null ou vide");

            // recupere la liste de cat en fonction de la langue
            Categories tmpCats = new Categories();
            tmpCats = this.categoriesList.GetOrAdd(langue, tmpCats);
            
            // recupere la categorie qui nous interesse
            foreach (Category tmpCat in tmpCats.categoriesList)
            {
                if (tmpCat.categoryId.Equals(catId))
                    return tmpCat;
            }

            twitterError.publishError("Category not found with parameters langue=" + langue + " catId=" + catId);
            throw new Exception("Category not found with parameters langue=" + langue + " catId="+catId);
        }

        // Summary:
        //      Cette method renvoie un critere donne pour un id de crit et une langue
        // Parameters:
        //      critId = id du critere, a recuperer dans la table criteria
        //      langue = langue en cours sur le site, a recuperer en session
        // Returns:
        //      le critere
        public Criteria getCriteriaLangId(String langue, int critId)
        {
            // teste la validite des parametres
            if (langue == null || String.Empty.Equals(langue.Trim()))
                throw new NullReferenceException("Erreur dans l'appel de la methode CategoryStorage.getCriteria, le parametre langue est null ou vide");

            // recupere la liste de cat en fonction de la langue
            ConcurrentDictionary<String, Criteria> tmpCrits = new ConcurrentDictionary<String, Criteria>();
            tmpCrits = this.criteriaList.GetOrAdd(langue, tmpCrits);

            // tweak pour recup tt les criteres pour une langue

            // recupere le critere qui nous interesse
            Criteria tmpCritLang = new Criteria();
            tmpCritLang = tmpCrits.GetOrAdd(critId.ToString(),tmpCritLang);

            if (tmpCritLang != null)
                return tmpCritLang;

            twitterError.publishError("Criteria not found with parameters langue=" + langue + " critId=" + critId);
            throw new Exception("Criteria not found with parameters langue=" + langue + " critId=" + critId);
        }

        // Summary:
        //      Cette method renvoie la liste des categorie sous la forme d'un fichier JSON
        // Parameters:
        //      langue = langue en cours sur le site, a recuperer en session
        // Returns:
        //      fichier JSON de type {"categoriesList":[{"ID":10,"LIB":"libelle test 10 fr"},{"ID":20,"LIB":"libelle test 20 fr"}]}	
        public String getCategoryListJSON(String language)
        {
            // code massivement pompe sur le msdn
            return JSONifye(this.getCategoriesByLanguage(language).categoriesList.ToArray());
        }

        // Summary:
        //      Cette method renvoie la liste des criteres sous la forme d'un fichier JSON
        // Parameters:
        //      langue = langue en cours sur le site, a recuperer en session
        // Returns:
        //      fichier JSON de type {"categoriesList":[{"ID":10,"LIB":"libelle test 10 fr"},{"ID":20,"LIB":"libelle test 20 fr"}]}	
        public String getCriteriaListJSON(String language)
        {
            // recupere la liste de cat en fonction de la langue
            ConcurrentDictionary<String, Criteria> tmpCrits = new ConcurrentDictionary<String, Criteria>();
            tmpCrits = this.criteriaList.GetOrAdd(language, tmpCrits);
            
            // code massivement pompe sur le msdn
            return JSONifye(tmpCrits.Values.ToArray());
        }

        // Summary:
        //      Cette methode retourne la liste de catgegorie en fonction d'une langue
        // Parameters:
        //      langue = langue en cours sur le site, a recuperer en session
        // Returns:
        //      on objet de type categorie
        public Categories getCategoriesByLanguage(String language)
        {
            Categories result = null;
            return this.categoriesList.GetOrAdd(language,result);
            return result;
        }

        // transforme un objet en chaine JSON
        public String JSONifye(Object _obj) {
            // code massivement pompe sur le msdn
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(_obj.GetType());
            ser.WriteObject(stream1, _obj);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            return sr.ReadToEnd();
        }

    }

    [Serializable]
    [DataContract]
    public class Categories
    {
        [DataMember]
        public List<Category> categoriesList;
        public String language;
    }

    [Serializable]
    [DataContract]
    public class Category
    {
        [DataMember(Name = "LIB")]
        public String libelle;
        [DataMember(Name = "ID")]
        public int categoryId;
        public Boolean active;
        public List<Criteria> criteriasList;
    }

    [Serializable]
    [DataContract]
    public class Criteria
    {
        [DataMember(Name = "ID")]
        public int criteriaId;
        [DataMember(Name = "LIB")]
        public String libelle;
        public Boolean active;
        public float weight;
    }

}