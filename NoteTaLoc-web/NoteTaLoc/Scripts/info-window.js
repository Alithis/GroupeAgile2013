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

function InfoWindow(map, options) {
    var me = this;
    me.setMap(map);
    me.options = $.extend(
		{
		    pixelOffsetY: 0,
		    padLeft: 10,
		    padRight: 10,
		    padTop: 10,
		    onCreated: null,
		    onClosed: null,
            urlTailImage:"Images/InfoWindowTail.png"
		},
		options);
    // Initialization
    me.divElement = null;
    me.marker = null;
    me.divContent = null;
    me.hIcon = 0;

    me.ready = function () {
        var pane = me.getPanes().floatPane;
        pane.appendChild(me.divElement);
        me.offsetX = ($(me.divElement).width() / 2) * -1;
        me.offsetY = ($(me.divElement).height()) * -1;

        var cancel = function cancelEvent(e) {
            var e = e || window.event;
            e.cancelBubble = true;
            if (e.stopPropagation) e.stopPropagation();
        }
        google.maps.event.addDomListener(me.divElement, 'mousedown', cancel);
        google.maps.event.addDomListener(me.divElement, 'click', cancel);
        google.maps.event.addDomListener(me.divElement, 'dblclick', cancel);
        google.maps.event.addDomListener(me.divElement, 'contextmenu', cancel);

        google.maps.event.addListener(this.getMap(), 'zoom_changed', function () {
            me.close();
        });

        $(me.divElement).bind('mousewheel', function (e) {
            e.stopPropagation();
        });

    }

    me.moveMap = function (w, h) {
        h += this.options.padTop;
        var map = this.getMap();
        var xOffset = 0, yOffset = 0;
        var latLng = me.marker.getPosition();
        var proj = map.getProjection();
        var proj = me.getProjection();
        var pt = proj.fromLatLngToContainerPixel(latLng);
        var mapWidth = $(map.getDiv()).width();

        yOffset = pt.y - h + me.options.pixelOffsetY;
        yOffset = yOffset < 0 ? yOffset : 0;

        xOffset = pt.x - w / 2 - this.options.padLeft;

        if (xOffset > 0) {
            if ((pt.x + w / 2 + this.options.padRight) > mapWidth) {
                xOffset = (pt.x + w / 2) - mapWidth + this.options.padRight;
            }
            else {
                xOffset = 0;
            }
        }
        if (xOffset != 0 || yOffset != 0) {
            map.panBy(xOffset, yOffset);
        }
    }

    me.CalculateMaxSize = function () {
        var mapDiv = $(me.getMap().getDiv());
        var hMax = me.options.padTop
                   + me.options.pixelOffsetY
                   + this.hIcon
                   + 19 //button close
                   + 52 //min height
                   + 13 //footer map
                   + 15; //pad bottom
        $(me.divContent).css({ "maxHeight": mapDiv.height() - hMax });

        var wMax = me.options.padLeft
                   + me.options.padRight
                   + 10; //margin
        $(me.divContent).css({ "maxWidth": mapDiv.width() - wMax });
    }

};

InfoWindow.prototype = new google.maps.OverlayView;

// Implement onAdd
InfoWindow.prototype.onAdd = function () {
    if (this.divElement) {
        this.ready();
    }
};

// Implement onRemove
InfoWindow.prototype.onRemove = function () {
    $(this.divElement).remove();
};

// Implement draw
InfoWindow.prototype.draw = function () {
    if (this.divElement) {
        var projection = this.getProjection();
        var position = projection.fromLatLngToDivPixel(this.get('position'));
        var div = this.divElement;
        div.style.left = position.x + this.offsetX + 'px';
        div.style.top = position.y + this.offsetY + this.options.pixelOffsetY + 'px';
    }
};

InfoWindow.prototype.resizeContent = function (w, h, callback) {
    var me = this;
    var firstChild = $(me.divContent);
    var actualWidth = firstChild.width();
    var actualHeight = firstChild.height();
    var maxHeight = parseFloat($(me.divContent).css("maxHeight"));
    var maxWidth = parseFloat($(me.divContent).css("maxWidth"));
    h = Math.min(h, maxHeight);
    w = Math.min(w, maxWidth);
    var dWidth = w - actualWidth;
    var dHeight = h - actualHeight;

    var newWidth = (dWidth > 0 ? "+=" : "-=") + Math.abs(dWidth);
    var newHeight = (dHeight > 0 ? "+=" : "-=") + Math.abs(dHeight);


    var left = $(this.divElement).position().left;
    var top = $(this.divElement).position().top;

    me.moveMap($(me.divElement).width() + dWidth, $(me.divElement).height() + dHeight);

    firstChild.animate(
	{
	    width: newWidth,
	    height: newHeight
	},
	{
	    duration: 200,
	    step: function (now, tween) {
	        if (tween.prop == "width") {
	            var l = left + (actualWidth - now) / 2;
	            $(me.divElement).css({ "left": l + "px" });
	        }
	        else if (tween.prop == "height") {
	            var t = actualHeight - now + top;
	            $(me.divElement).css({ "top": t + "px" });
	        }
	    },
	    complete: function () {
	        me.offsetX -= dWidth / 2;
	        me.offsetY -= dHeight;
	        if ($.isFunction(callback)) {
	            callback();
	        }

	    }
	});
}

InfoWindow.prototype.close = function () {
    if (this.marker != null) {
        this.unbindAll();
        $(this.divElement).hide();
        this.marker = null;
        if ($.isFunction(this.options.onClosed)) {
            this.options.onClosed();
        }
    }
};

InfoWindow.prototype.toogle = function (content, marker) {
    if (this.marker != null && marker.__gm_id == this.marker.__gm_id) {
        this.close();
    }
    else {
        if (this.marker != null) {
            this.close();
        }
        this.show(content, marker);
    }
};

InfoWindow.prototype.show = function (content, marker) {
    var me = this;
    me.marker = marker;
    me.hIcon = marker.getIcon().size.height;
    if (me.divElement == null) {
        var divWindow = $("<div/>").addClass("info-window")
                                   .css({ "width": "100%", "height": "100%", "paddingTop": "6px", "position": "relative", "cursor": "default", "overflow": "auto" });
        var div = $("<div/>").css({ "position": "absolute", "display": "none" })
						 .css({ "padding": "0px 0px 19px 0px" })
						 .append($("<div/>").css({ "background": "transparent url('" + me.options.urlTailImage + "') no-repeat center bottom", "position": "absolute", "width": "100%", "height": "20px", "bottom": "0px", "zIndex": "10" }))
						 .append(divWindow)
						 .append($("<div/>").addClass("info-window-close")
                                            .css({ "position": "absolute" })
											.mouseover(function () {
											    $(this).addClass("info-window-close-over");
											})
											.mouseout(function () {
											    $(this).removeClass("info-window-close-over");
											})
											.click(function (event) {
											    event.stopPropagation();
											    me.close();
											}));
        me.divElement = div[0];
        me.ready();

        me.divContent = $("<div/>").css({ "margin": "15px 5px 5px 5px" })
                                   .appendTo(divWindow);
    }

    me.divContent.html(content).css({ "width": "", "height": "" });

    me.bindTo('position', marker);
    me.bindTo('visible', marker);
    me.bindTo('zIndex', marker);

    me.offsetX = ($(me.divElement).width() / 2) * -1;
    me.offsetY = ($(me.divElement).height()) * -1;
    me.draw();

    me.CalculateMaxSize();

    $(me.divElement).fadeIn(function () {
        if ($.isFunction(me.options.onCreated)) {
            me.options.onCreated(me.divContent);
        }
    });
    me.moveMap($(me.divElement).width(), $(me.divElement).height());
}