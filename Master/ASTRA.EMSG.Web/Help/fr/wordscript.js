
function msoCommentShow(anchor_id, com_id)
{
	if(msoBrowserCheck()) 
		{
		c = document.all(com_id);
		a = document.all(anchor_id);
		if (null != c && null == c.length && null != a && null == a.length)
			{
			var cw = c.offsetWidth;
			var ch = c.offsetHeight;
			var aw = a.offsetWidth;
			var ah = a.offsetHeight;
			var x  = a.offsetLeft;
			var y  = a.offsetTop;
			var el = a;
			while (el.tagName != "BODY") 
				{
				el = el.offsetParent;
				x = x + el.offsetLeft;
				y = y + el.offsetTop;
				}
			var bw = document.body.clientWidth;
			var bh = document.body.clientHeight;
			var bsl = document.body.scrollLeft;
			var bst = document.body.scrollTop;
			if (x + cw + ah / 2 > bw + bsl && x + aw - ah / 2 - cw >= bsl ) 
				{ c.style.left = x + aw - ah / 2 - cw; }
			else 
				{ c.style.left = x + ah / 2; }
			if (y + ch + ah / 2 > bh + bst && y + ah / 2 - ch >= bst ) 
				{ c.style.top = y + ah / 2 - ch; }
			else 
				{ c.style.top = y + ah / 2; }
			c.style.visibility = "visible";
}	}	}
function msoCommentHide(com_id) 
{
	if(msoBrowserCheck())
		{
		c = document.all(com_id);
		if (null != c && null == c.length)
		{
		c.style.visibility = "hidden";
		c.style.left = -1000;
		c.style.top = -1000;
		} } 
}
function msoBrowserCheck()
{
	ms = navigator.appVersion.indexOf("MSIE");
	vers = navigator.appVersion.substring(ms + 5, ms + 6);
	ie4 = (ms > 0) && (parseInt(vers) >= 4);
	return ie4;
}
if (msoBrowserCheck())
{
	document.styleSheets.dynCom.addRule(".msocomanchor","background: infobackground");
	document.styleSheets.dynCom.addRule(".msocomoff","display: none");
	document.styleSheets.dynCom.addRule(".msocomtxt","visibility: hidden");
	document.styleSheets.dynCom.addRule(".msocomtxt","position: absolute");
	document.styleSheets.dynCom.addRule(".msocomtxt","top: -1000");
	document.styleSheets.dynCom.addRule(".msocomtxt","left: -1000");
	document.styleSheets.dynCom.addRule(".msocomtxt","width: 33%");
	document.styleSheets.dynCom.addRule(".msocomtxt","background: infobackground");
	document.styleSheets.dynCom.addRule(".msocomtxt","color: infotext");
	document.styleSheets.dynCom.addRule(".msocomtxt","border-top: 1pt solid threedlightshadow");
	document.styleSheets.dynCom.addRule(".msocomtxt","border-right: 2pt solid threedshadow");
	document.styleSheets.dynCom.addRule(".msocomtxt","border-bottom: 2pt solid threedshadow");
	document.styleSheets.dynCom.addRule(".msocomtxt","border-left: 1pt solid threedlightshadow");
	document.styleSheets.dynCom.addRule(".msocomtxt","padding: 3pt 3pt 3pt 3pt");
	document.styleSheets.dynCom.addRule(".msocomtxt","z-index: 100");
}
