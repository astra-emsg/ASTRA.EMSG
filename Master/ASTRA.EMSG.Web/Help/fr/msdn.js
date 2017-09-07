window.onload = resize;
window.onresize = resize;
 
function resize() {
	if (document.body.clientWidth == 0) return;

	var pageHeader = document.all("PageHeader");
	var pageFooter = document.all("PageFooter");
	var pageBody   = document.all("PageBody");

	var bWidth  = document.body.offsetWidth - 4;
 	var bHeight = Math.max(document.body.offsetHeight - pageHeader.offsetHeight - pageFooter.offsetHeight - 4, 0);
	pageBody.style.width  = bWidth;
 	pageBody.style.height = bHeight;

  try { 	
   	var images = pageBody.getElementsByTagName("IMG");
   	for (var i=0; i<images.length; i++) {
   	  var image = images[i];
   	  image.style.border = image.style.border;
    }
  }
  catch(e) {
  }

	try {
		pageBody.setActive();
	}
	catch(e) {
	}
}

function window.onbeforeprint() {
	var pageBody   = document.all("PageBody");
 	pageBody.style.overflow = "visible";
}

function window.onafterprint() {
	var pageBody   = document.all("PageBody");
 	pageBody.style.overflow = "auto";
  resize();
}
