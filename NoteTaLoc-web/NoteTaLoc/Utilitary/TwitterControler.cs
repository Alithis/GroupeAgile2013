using NoteTaLoc.Models;
using System;
using System.Configuration;
using System.Web.Configuration;
using TweetSharp;

namespace NoteTaLoc.Utilitary
{
    public class TwitterInfo
    {
        private readonly Configuration config;
        private readonly String infoMessageNote;
        private readonly TwitterService twitterService;

        public TwitterInfo(Configuration conf)
        {
            config = conf;
            twitterService = new TwitterService(config.AppSettings.Settings["ConsumerKeyInfo"].Value,
                                                config.AppSettings.Settings["ConsumerSecretInfo"].Value);
            twitterService.AuthenticateWith(config.AppSettings.Settings["AccessTokenInfo"].Value,
                                            config.AppSettings.Settings["AccessTokenSecretInfo"].Value);
            infoMessageNote = config.AppSettings.Settings["NotificationNouvelleNote"].Value;
        }

        public String publishNewNote(NoteTable note, AdresseTable adresse)
        {
            var tweettOption = new SendTweetOptions();
            String tweet = infoMessageNote + note.Note + "* addresse:  " + adresse.AdresseLine;
            int length = 139;
            if (tweet.Length < 139)
            {
                length = tweet.Length;
            }
            tweettOption.Status = tweet.Substring(0, length);
            TwitterStatus status = twitterService.SendTweet(tweettOption);
            return twitterService.Response.Response;
        }
    }

    public class TwitterError
    {
        private Configuration config;
        private TwitterService twitterService;
        private String infoErreurGeneral;

        public TwitterError(Configuration conf)
        {
            config = conf;
            twitterService = new TwitterService(config.AppSettings.Settings["ConsumerKeyError"].Value,
                                                config.AppSettings.Settings["ConsumerSecretError"].Value);
            twitterService.AuthenticateWith(config.AppSettings.Settings["AccessTokenError"].Value,
                                            config.AppSettings.Settings["AccessTokenSecretError"].Value);
            infoErreurGeneral = config.AppSettings.Settings["NotificationErreur"].Value;
        }

        public String publishError(String message)
        {
            var tweettOption = new SendTweetOptions();
            String tweet = message;
            int length = 139;
            if (tweet.Length < 139)
            {
                length = tweet.Length;
            }
            tweettOption.Status = tweet.Substring(0, length);

            TwitterStatus status = twitterService.SendTweet(tweettOption);
            return twitterService.Response.Response;
        }
    }
}