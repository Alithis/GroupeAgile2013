/**
* @name InfoWindow
* @version 1.0.0 [7 Juin 2013]
* @author LANDE Christophe
* @copyright Copyright 2013 LANDE Christophe

* compiled by http://closure-compiler.appspot.com

/*!
*
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation; either version 2 of the License, or
* (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
* 
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
* 
*/

/*required libraries: jquery, google maps api */

/**
* @map : map element from google maps api
* @options: This class represents the optional parameter passed to the {@link InfoBox} constructor.
*   @property {pixelOffsetY|number} y offset of the info-window (default:0)
*   @property {padLeft|number} define the space between the info-window left border and the map border left (default:10)
*   @property {padRight|number} define the space between the info-window right border and the map border right (default:10)
*   @property {padTop|number} define the space between the info-window top border and the map border top (default:10)
*   @property {onCreated|function} callback function after the info-window displaying; parameter: the content div element
*   @property {onClosed|function} callback function after the info-window closing;
*   @property {urlTailImage|string} url of tail image;
**/

/**
/* @show: show the info-window
/*   @param {content|string} string containing the info-window content
/*   @param {marker} the marker linked to the info-indow
/* @close: close the info-window
/* @toogle: switch the info-window display
/*   @param {content|string} string containing the info-window content
/*   @param {marker} the marker linked to the info-indow
/* @resizeContent: resize the info-window content
/*   @param {w|number} the new width of the info-window content
/*   @param {h|number} the new height of the info-window content
/*   @param {callback|function} callback function after the info-window resizing;
**/
function InfoWindow(d, b) {
    var a = this; a.setMap(d); a.options = $.extend({ pixelOffsetY: 0, padLeft: 10, padRight: 10, padTop: 10, onCreated: null, onClosed: null, urlTailImage: "Images/InfoWindowTail.png" }, b); a.divElement = null; a.marker = null; a.divContent = null; a.hIcon = 0; a.ready = function () {
        a.getPanes().floatPane.appendChild(a.divElement); a.offsetX = -1 * ($(a.divElement).width() / 2); a.offsetY = -1 * $(a.divElement).height(); var c = function (a) { a = a || window.event; a.cancelBubble = !0; a.stopPropagation && a.stopPropagation() }; google.maps.event.addDomListener(a.divElement,
"mousedown", c); google.maps.event.addDomListener(a.divElement, "click", c); google.maps.event.addDomListener(a.divElement, "dblclick", c); google.maps.event.addDomListener(a.divElement, "contextmenu", c); google.maps.event.addListener(this.getMap(), "zoom_changed", function () { a.close() })
    }; a.moveMap = function (c, b) {
        b += this.options.padTop; var d = this.getMap(), f = 0, e = 0, f = a.marker.getPosition(), e = d.getProjection(), e = a.getProjection(), h = e.fromLatLngToContainerPixel(f), g = $(d.getDiv()).width(), e = h.y - b + a.options.pixelOffsetY,
e = 0 > e ? e : 0, f = h.x - c / 2 - this.options.padLeft; 0 < f && (f = h.x + c / 2 + this.options.padRight > g ? h.x + c / 2 - g + this.options.padRight : 0); (0 != f || 0 != e) && d.panBy(f, e)
    }; a.CalculateMaxSize = function () { var c = $(a.getMap().getDiv()), b = a.options.padTop + a.options.pixelOffsetY + this.hIcon + 19 + 52 + 13 + 15; $(a.divContent).css({ maxHeight: c.height() - b }); b = a.options.padLeft + a.options.padRight + 10; $(a.divContent).css({ maxWidth: c.width() - b }) } 
} InfoWindow.prototype = new google.maps.OverlayView;
InfoWindow.prototype.onAdd = function () { this.divElement && this.ready() }; InfoWindow.prototype.onRemove = function () { $(this.divElement).remove() }; InfoWindow.prototype.draw = function () { if (this.divElement) { var d = this.getProjection().fromLatLngToDivPixel(this.get("position")), b = this.divElement; b.style.left = d.x + this.offsetX + "px"; b.style.top = d.y + this.offsetY + this.options.pixelOffsetY + "px" } };
InfoWindow.prototype.resizeContent = function (d, b, a) {
    var c = this, j = $(c.divContent), l = j.width(), f = j.height(), e = parseFloat($(c.divContent).css("maxHeight")), h = parseFloat($(c.divContent).css("maxWidth")); b = Math.min(b, e); d = Math.min(d, h); var g = d - l, k = b - f; d = (0 < g ? "+=" : "-=") + Math.abs(g); b = (0 < k ? "+=" : "-=") + Math.abs(k); var m = $(this.divElement).position().left, n = $(this.divElement).position().top; c.moveMap($(c.divElement).width() + g, $(c.divElement).height() + k); j.animate({ width: d, height: b }, { duration: 200, step: function (a,
b) { if ("width" == b.prop) { var d = m + (l - a) / 2; $(c.divElement).css({ left: d + "px" }) } else "height" == b.prop && (d = n + (f - a), $(c.divElement).css({ top: d + "px" })) }, complete: function () { c.offsetX -= g / 2; c.offsetY -= k; $.isFunction(a) && a() } 
    })
}; InfoWindow.prototype.close = function () { if (null != this.marker && (this.unbindAll(), $(this.divElement).hide(), this.marker = null, $.isFunction(this.options.onClosed))) this.options.onClosed() };
InfoWindow.prototype.toogle = function (d, b) { null != this.marker && b.__gm_id == this.marker.__gm_id ? this.close() : (null != this.marker && this.close(), this.show(d, b)) };
InfoWindow.prototype.show = function (d, b) {
    var a = this; a.marker = b; a.hIcon = b.getIcon().size.height; if (null == a.divElement) {
        var c = $("<div/>").addClass("info-window").css({ width: "100%", height: "100%", paddingTop: "6px", position: "relative", cursor: "default", overflow: "auto" }), j = $("<div/>").css({ position: "absolute", display: "none" }).css({ padding: "0px 0px 19px 0px" }).append($("<div/>").css({ background: "transparent url('" + a.options.urlTailImage + "') no-repeat center bottom", position: "absolute", width: "100%", height: "20px",
            bottom: "0px", zIndex: "10"
        })).append(c).append($("<div/>").addClass("info-window-close").css({ position: "absolute" }).mouseover(function () { $(this).addClass("info-window-close-over") }).mouseout(function () { $(this).removeClass("info-window-close-over") }).click(function (b) { b.stopPropagation(); a.close() })); a.divElement = j[0]; a.ready(); a.divContent = $("<div/>").css({ margin: "15px 5px 5px 5px" }).appendTo(c)
    } a.divContent.html(d).css({ width: "", height: "" }); a.bindTo("position", b); a.bindTo("visible", b); a.bindTo("zIndex",
b); a.offsetX = -1 * ($(a.divElement).width() / 2); a.offsetY = -1 * $(a.divElement).height(); a.draw(); a.CalculateMaxSize(); $(a.divElement).fadeIn(function () { if ($.isFunction(a.options.onCreated)) a.options.onCreated(a.divContent) }); a.moveMap($(a.divElement).width(), $(a.divElement).height())
};