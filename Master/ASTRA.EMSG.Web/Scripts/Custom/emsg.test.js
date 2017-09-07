Emsg.Test = function(name)
{
  this.name = name;
  Emsg.Test.add(this);
};
Emsg.Test.add = function(test)
{
  var tests = Emsg.Test.tests || [];
  tests.push(test);
  Emsg.Test.tests = tests;
};
Emsg.Test.runAll = function () {
  var run = 0, tests = Emsg.Test.tests || [];
  Emsg.Test.asserts = 0;
  for (var i = 0; i < tests.length; ++i) { tests[i].run(); ++run };
  if (window.console && window.console.log) {
    console.log("Run " + run + " Tests. Assertions: " + Emsg.Test.asserts);
  }
};
Emsg.Test.prototype.fail = function(error) { throw error; };
Emsg.Test.prototype.assert = function(condition, msg)
{
  this.assertEqual(true, condition);
};
Emsg.Test.prototype.assertEqual = function(a, b, msg)
{
  ++Emsg.Test.asserts;
  var error = "Expected '" + a + "' but was '" + b + "'";
  if ( msg !== undefined ) error += " : " + msg;
  if ( a !== b ) this.fail(error);
};
Emsg.Test.prototype.run = function()
{
  this.fail("not yet implemented.");
};


new Emsg.Test("Emsg.Util.split").run = function () {
  var wkt = OpenLayers.Geometry.fromWKT;
  var line = wkt("LINESTRING(0 0,10 0,30 0,30 30)");

  var split;
  split = Emsg.Util.split(line, wkt("POINT(0 10)"));
  this.assert(!split);

  split = Emsg.Util.split(line, wkt("POINT(0 0)"));
  this.assert(!split.head);
  this.assertEqual("LINESTRING(0 0,10 0,30 0,30 30)", split.tail.toString());

  split = Emsg.Util.split(line, wkt("POINT(10 0)"));
  this.assertEqual("LINESTRING(0 0,10 0)", split.head.toString());
  this.assertEqual("LINESTRING(10 0,30 0,30 30)", split.tail.toString());

  split = Emsg.Util.split(line, wkt("POINT(5 0)"));
  this.assertEqual("LINESTRING(0 0,5 0)", split.head.toString());
  this.assertEqual("LINESTRING(5 0,10 0,30 0,30 30)", split.tail.toString());

  split = Emsg.Util.split(line, wkt("POINT(20 0)"));
  this.assertEqual("LINESTRING(0 0,10 0,20 0)", split.head.toString());
  this.assertEqual("LINESTRING(20 0,30 0,30 30)", split.tail.toString());

  split = Emsg.Util.split(line, wkt("POINT(30 30)"));
  this.assertEqual("LINESTRING(0 0,10 0,30 0,30 30)", split.head.toString());
  this.assert(!split.tail);
};

new Emsg.Test("Emsg.Util.cutSegment").run = function () {
  var wkt = OpenLayers.Geometry.fromWKT;
  var line = wkt("LINESTRING(0 0,10 0,30 0,30 30,0 0)");
  var cut = Emsg.Util.cutSegment(line, wkt("POINT(10 0)"), wkt("POINT(30 30)"));
  this.assertEqual("LINESTRING(0 0,10 0)"       , cut.head.toString());
  this.assertEqual("LINESTRING(10 0,30 0,30 30)", cut.body.toString());
  this.assertEqual("LINESTRING(30 30,0 0)"      , cut.tail.toString());
  var cut = Emsg.Util.cutSegment(line, wkt("POINT(0 0)"), wkt("POINT(0 0)"));
  this.assertEqual(line.toString(), cut.body.toString());
};

new Emsg.Test("Emsg.Util.mergeDuplicateNodes").run = function () {
  var wkt = OpenLayers.Geometry.fromWKT;
  var line = wkt("MULTILINESTRING((0 0,10 0,20 0),(20 0,30 0,30 30))"); Emsg.Util.mergeDuplicateNodes(line);
  this.assertEqual("MULTILINESTRING((0 0,10 0,20 0,30 0,30 30))", line.toString());

  line = wkt("MULTILINESTRING((0 0,10 0,20 0,30 0,30 30,0 0))"); Emsg.Util.mergeDuplicateNodes(line);
  this.assertEqual("MULTILINESTRING((0 0,10 0,20 0,30 0,30 30,0 0))", line.toString());

  line = wkt("MULTILINESTRING((0 0,10 0,20 0),(20 0,30 0,30 30,0 0))"); Emsg.Util.mergeDuplicateNodes(line);
  this.assertEqual("MULTILINESTRING((0 0,10 0,20 0,30 0,30 30,0 0))", line.toString());

  line = wkt("MULTILINESTRING((0 0,10 0,20 0),(0 0,30 30,30 0,20 0))"); Emsg.Util.mergeDuplicateNodes(line);
  this.assertEqual("MULTILINESTRING((0 0,10 0,20 0,30 0,30 30,0 0))", line.toString());

  line = wkt("MULTILINESTRING((20 0,10 0,0 0),(20 0,30 0,30 30,0 0))"); Emsg.Util.mergeDuplicateNodes(line);
  this.assertEqual("MULTILINESTRING((20 0,10 0,0 0,30 30,30 0,20 0))", line.toString());

  line = wkt("MULTILINESTRING((20 0,10 0,0 0),(0 0,30 30,30 0,20 0))"); Emsg.Util.mergeDuplicateNodes(line);
  this.assertEqual("MULTILINESTRING((20 0,10 0,0 0,30 30,30 0,20 0))", line.toString());

  line = wkt("MULTILINESTRING((20 0,30 0,30 30),(0 0,10 0,20 0))"); Emsg.Util.mergeDuplicateNodes(line);
  this.assertEqual("MULTILINESTRING((30 30,30 0,20 0,10 0,0 0))", line.toString());
};


new Emsg.Test("Emsg.Feature").run = function () {
  var wkt = OpenLayers.Geometry.fromWKT;
  var line = wkt("LINESTRING(0 0,10 0,30 0,30 30)");
  var a, b, p;
  p = new OpenLayers.Feature.Vector.Cutable(line);
  a = new OpenLayers.Feature.Vector.Segment();
  b = new OpenLayers.Feature.Vector.Segment();
  this.assertEqual("MULTILINESTRING((0 0,10 0,30 0,30 30))", p.geometry.toString());
  this.assertEqual("MULTILINESTRING((0 0,10 0,30 0,30 30))", p.getFreeGeometry().toString());
  a.setStart(wkt("POINT(0 0)"));
  a.setEnd(wkt("POINT(0 0)"));
  a.setSketch(true);
  this.assertEqual(null, a.geometry);
  p.addChild(a);
  this.assertEqual("MULTILINESTRING((0 0,10 0,30 0,30 30))", p.getFreeGeometry().toString());
  a.setEnd(wkt("POINT(10 0)"));
  this.assertEqual(null, a.geometry);
  a.setSketch(false);
  this.assertEqual(null, a.geometry);
  this.assertEqual("MULTILINESTRING((10 0,30 0,30 30))", p.getFreeGeometry().toString());
  a.updateGeometry();
  this.assertEqual("LINESTRING(0 0,10 0)", a.geometry.toString());

  b.setStart(wkt("POINT(30 30)"));
  b.setEnd(wkt("POINT(30 30)"));
  b.setSketch(true);
  this.assertEqual(null, b.geometry);
  p.addChild(b);
  this.assertEqual("MULTILINESTRING((10 0,30 0,30 30))", p.getFreeGeometry().toString());
  b.setEnd(wkt("POINT(10 0)"));
  b.setSketch(false);
  this.assertEqual(null, b.geometry);
  this.assertEqual(null, p.getFreeGeometry());
  b.updateGeometry();
  this.assertEqual("LINESTRING(10 0,30 0,30 30)", b.geometry.toString());
  this.assert(a.geometry.components[1].id != b.geometry.components[0].id);
};


new Emsg.Test("Emsg.Util.createSegment").run = function () {
  var wkt = OpenLayers.Geometry.fromWKT;
  var line = wkt("LINESTRING(0 0,10 0,30 0,30 30)");
  var p, s;
  p = new OpenLayers.Feature.Vector.Cutable(line);
  s = Emsg.Util.createSegment(p, wkt("POINT(5 0)"));
  this.assertEqual(null, p.getFreeGeometry());
  this.assertEqual("LINESTRING(0 0,10 0,30 0,30 30)", s.geometry.toString());
  this.assertEqual("POINT(0 0)", s.start.toString());
  this.assertEqual("POINT(30 30)", s.end.toString());

  p = new OpenLayers.Feature.Vector.Cutable(wkt("MULTILINESTRING((0 0,10 0),(30 0,30 30))"));
  s = Emsg.Util.createSegment(p, wkt("POINT(5 0)"));
  this.assertEqual("MULTILINESTRING((30 0,30 30))", p.getFreeGeometry().toString());
  this.assertEqual("LINESTRING(0 0,10 0)", s.geometry.toString());
  this.assertEqual("POINT(0 0)", s.start.toString());
  this.assertEqual("POINT(10 0)", s.end.toString());
  s = Emsg.Util.createSegment(p, wkt("POINT(30 10)"));
  this.assertEqual(null, p.getFreeGeometry());
  this.assertEqual("LINESTRING(30 0,30 30)", s.geometry.toString());
  this.assertEqual("POINT(30 0)", s.start.toString());
  this.assertEqual("POINT(30 30)", s.end.toString());
};


new Emsg.Test("Emsg.Util.createSegments").run = function () {
  var wkt = OpenLayers.Geometry.fromWKT;
  var line = wkt("LINESTRING(0 0,10 0,30 0,30 30)");
  var p, s;
  p = new OpenLayers.Feature.Vector.Cutable(line);
  s = Emsg.Util.createSegments(p, wkt("POINT(5 0)"));
  this.assertEqual(null, p.getFreeGeometry());
  this.assertEqual(1, s.length);
  this.assertEqual("LINESTRING(0 0,10 0,30 0,30 30)", s[0].geometry.toString());
  this.assertEqual("POINT(0 0)", s[0].start.toString());
  this.assertEqual("POINT(30 30)", s[0].end.toString());

  p = new OpenLayers.Feature.Vector.Cutable(wkt("MULTILINESTRING((0 0,10 0),(30 0,30 30))"));
  s = Emsg.Util.createSegments(p, wkt("POINT(5 0)"));
  this.assertEqual(null, p.getFreeGeometry());
  this.assertEqual(2, s.length);
  this.assertEqual("LINESTRING(0 0,10 0)", s[0].geometry.toString());
  this.assertEqual("POINT(0 0)", s[0].start.toString());
  this.assertEqual("POINT(10 0)", s[0].end.toString());
  this.assertEqual("LINESTRING(30 0,30 30)", s[1].geometry.toString());
  this.assertEqual("POINT(30 0)", s[1].start.toString());
  this.assertEqual("POINT(30 30)", s[1].end.toString());
  s[1].setEnd(wkt("POINT(30 10)"));
  s[1].updateGeometry(wkt("POINT(30 10)"));
  this.assertEqual("MULTILINESTRING((30 10,30 30))", p.getFreeGeometry().toString());
  this.assertEqual("MULTILINESTRING((0 0,10 0),(30 0,30 30))", p.geometry.toString());

  p = new OpenLayers.Feature.Vector.Cutable(wkt("MULTILINESTRING((0 0,10 0),(30 0,30 30))"));
  p.attributes.hasOtherChildren = true;
  s = Emsg.Util.createSegments(p, wkt("POINT(5 0)"));
  this.assertEqual("MULTILINESTRING((30 0,30 30))", p.getFreeGeometry().toString());
  this.assertEqual(1, s.length);
  this.assertEqual("LINESTRING(0 0,10 0)", s[0].geometry.toString());
  this.assertEqual("POINT(0 0)", s[0].start.toString());
  this.assertEqual("POINT(10 0)", s[0].end.toString());

  p = new OpenLayers.Feature.Vector.Cutable(wkt("MULTILINESTRING((0 0,10 0),(30 0,30 30))"));
  p.attributes.hasOtherChildren = true;
  s = Emsg.Util.createSegments(p, wkt("POINT(5 0)"));
  this.assertEqual("MULTILINESTRING((30 0,30 30))", p.getFreeGeometry().toString());
  this.assertEqual(1, s.length);
  this.assertEqual("LINESTRING(0 0,10 0)", s[0].geometry.toString());
  this.assertEqual("POINT(0 0)", s[0].start.toString());
  this.assertEqual("POINT(10 0)", s[0].end.toString());
  s[0].setStart(wkt("POINT(5 0)"));
  s[0].updateGeometry();
  this.assertEqual("MULTILINESTRING((0 0,10 0),(30 0,30 30))", p.geometry.toString());
  this.assertEqual("MULTILINESTRING((30 0,30 30),(0 0,5 0))", p.getFreeGeometry().toString());

  p = new OpenLayers.Feature.Vector.Cutable(wkt("MULTILINESTRING((0 0,10 0,20 0),(20 0,30 0,0 0))"), { mergeDuplicateNodes : true });
  s = Emsg.Util.createSegments(p, wkt("POINT(5 0)"));
  this.assertEqual("MULTILINESTRING((0 0,10 0,20 0,30 0,0 0))", p.geometry.toString());
  this.assertEqual(1, s.length);
  this.assertEqual("LINESTRING(0 0,10 0,20 0,30 0,0 0)", s[0].geometry.toString());

  p = new OpenLayers.Feature.Vector.Cutable(wkt("MULTILINESTRING((0 0,10 0,20 0,30 0,40 0))"));
  var c = new OpenLayers.Feature.Vector.Segment(wkt("LINESTRING(15 0,35 0)"));
  this.assertEqual("LINESTRING(15 0,35 0)", c.geometry.toString());
  p.addChild(c);
  c.updateGeometry();
  this.assertEqual("LINESTRING(15 0,20 0,30 0,35 0)", c.geometry.toString());
  this.assertEqual("MULTILINESTRING((0 0,10 0,15 0),(35 0,40 0))", p.getFreeGeometry().toString());

  s = Emsg.Util.createSegments(p, wkt("POINT(5 0)"));
  this.assertEqual(2, s.length);
  this.assertEqual("LINESTRING(0 0,10 0,15 0)", s[0].geometry.toString());
  this.assertEqual("LINESTRING(35 0,40 0)", s[1].geometry.toString());

  p = new OpenLayers.Feature.Vector.Cutable(wkt("MULTILINESTRING((0 0,10 0,20 0,30 0,40 0))"));
  c = new OpenLayers.Feature.Vector.Segment(wkt("LINESTRING(15 0,35 0)"));
  p.addChild(c);
  this.assertEqual("MULTILINESTRING((0 0,10 0,15 0),(35 0,40 0))", p.getFreeGeometry().toString());

  s = Emsg.Util.createSegments(p, wkt("POINT(37 0)"));
  this.assertEqual(2, s.length);
  this.assertEqual("LINESTRING(0 0,10 0,15 0)", s[0].geometry.toString());
  this.assertEqual("LINESTRING(35 0,40 0)", s[1].geometry.toString());
};


new Emsg.Test("Emsg.createDragFeature").run = function () {
  var wkt = OpenLayers.Geometry.fromWKT;
  var line = wkt("LINESTRING(0 0,1 1,2 0,3 5)");
  var p, s;
  p = new OpenLayers.Feature.Vector.Segment(line);
  s = Emsg.Util.createDragFeatures(p);
  this.assertEqual(0, s.length);
  p.parent = true;
  s = Emsg.Util.createDragFeatures(p);
  this.assertEqual(2, s.length);
  this.assertEqual("POINT(0 0)", s[0].geometry.toString());
  this.assertEqual("POINT(3 5)", s[1].geometry.toString());

  this.assertEqual(p, s[0].attributes.ref);
  this.assertEqual(p, s[1].attributes.ref);

  this.assertEqual("start", s[0].attributes.type);
  this.assertEqual("end", s[1].attributes.type);
};


new Emsg.Test("Emsg.Util.mergeSegment").run = function () {
  var wkt = OpenLayers.Geometry.fromWKT;
  var a, b, c, s, p;
  p = new OpenLayers.Feature.Vector.Cutable(wkt("LINESTRING(0 0,10 0,30 0,30 30)"));
  a = new OpenLayers.Feature.Vector.Segment(wkt("LINESTRING(0 0, 10 0)"));
  b = new OpenLayers.Feature.Vector.Segment(wkt("LINESTRING(30 0, 30 30)"));
  c = new OpenLayers.Feature.Vector.Segment(wkt("LINESTRING(10 0, 30 0)"));
  p.addChild(a);
  p.addChild(b);
  p.addChild(c);
  this.assertEqual(3, p.children.length);
  this.assertEqual(null, p.getFreeGeometry());
  var removed = Emsg.Util.mergeSegment(c);
  this.assertEqual(2, removed.length);
  this.assertEqual(a, removed[0]);
  this.assertEqual(b, removed[1]);
  this.assertEqual("LINESTRING(0 0,10 0,30 0,30 30)", c.geometry.toString());

  p = new OpenLayers.Feature.Vector.Cutable(wkt("MULTILINESTRING((0 0,10 0,20 0),(20 5,30 0,30 30))"));
  a = new OpenLayers.Feature.Vector.Segment(wkt("LINESTRING(0 0,10 0,20 0)"));
  b = new OpenLayers.Feature.Vector.Segment(wkt("LINESTRING(20 5,30 0,30 30)"));
  p.addChild(a);
  p.addChild(b);
  this.assertEqual(2, p.children.length);
  this.assertEqual(null, p.getFreeGeometry());
  var removed = Emsg.Util.mergeSegment(b);
  this.assertEqual(0, removed.length);

  p = new OpenLayers.Feature.Vector.Cutable(wkt("MULTILINESTRING((0 0,10 0,20 0),(20 0,30 0,0 0))"), { mergeDuplicateNodes : true });
  a = new OpenLayers.Feature.Vector.Segment(wkt("LINESTRING(0 0,10 0,20 0)"));
  b = new OpenLayers.Feature.Vector.Segment(wkt("LINESTRING(20 0,30 0,0 0)"));
  p.addChild(a);
  p.addChild(b);
  this.assertEqual(2, p.children.length);
  this.assertEqual(null, p.getFreeGeometry());
  var removed = Emsg.Util.mergeSegment(b);
  this.assertEqual(1, removed.length);
  this.assertEqual("LINESTRING(0 0,10 0,20 0,30 0,0 0)", b.geometry.toString());
};


new Emsg.Test("Emsg.Feature.findReference").run = function () {
  var wkt = OpenLayers.Geometry.fromWKT;
  var a, b, p;

  p = new OpenLayers.Feature.Vector.Cutable(wkt("MULTILINESTRING((0 0,10 0,20 0),(20 0,30 0,30 30,0 0))"));
  a = new OpenLayers.Feature.Vector.Segment(wkt("LINESTRING(0 0,20 0)"));
  b = new OpenLayers.Feature.Vector.Segment(wkt("LINESTRING(20 0,0 0)"));
  p.addChild(a);
  p.addChild(b);
  this.assertEqual(2, p.children.length);
  this.assertEqual(null, p.getFreeGeometry());
  this.assertEqual(p.geometry.components[0], a.findReference());
  this.assertEqual(p.geometry.components[0].id, a.findReference().id);
  this.assertEqual(p.geometry.components[0], b.findReference());
  this.assertEqual(p.geometry.components[0].id, b.findReference().id);
};

new Emsg.Test("Emsg.Util.getFreeGeometry").run = function () {
  var wkt = OpenLayers.Geometry.fromWKT;
  var f, g, c = [];

  g = wkt("MULTILINESTRING((0 0,10 0,20 0),(20 0,30 0,30 30,40 40))");
  f = Emsg.Util.getFreeGeometry;
  this.assertEqual(g.toString(), f(g, c).toString());

  c.push({ geometry: wkt("LINESTRING(20 0,40 40)"), fid: 1 });
  this.assertEqual(wkt("MULTILINESTRING((0 0,10 0,20 0))").toString(), f(g, c).toString());

  c.push({ geometry: wkt("LINESTRING(10 0,20 0)"), fid: 2 });
  this.assertEqual(wkt("MULTILINESTRING((0 0,10 0))").toString(), f(g, c).toString());

  c.push({ geometry: wkt("LINESTRING(0 0,10 0)"), fid: 3 });
  this.assertEqual(null, f(g, c));

  this.assertEqual(wkt("MULTILINESTRING((20 0,30 0,30 30,40 40))").toString(), f(g, c, 1).toString());
  this.assertEqual(wkt("MULTILINESTRING((10 0,20 0))").toString(), f(g, c, 2).toString());
  this.assertEqual(wkt("MULTILINESTRING((0 0,10 0))").toString(), f(g, c, 3).toString());

  c = []; g = wkt("MULTILINESTRING((0 0,10 0,20 0,30 0,30 30,40 40))");
  c.push({ geometry: wkt("LINESTRING(10 0,30 0)"), fid: 2 });
  this.assertEqual(wkt("MULTILINESTRING((0 0,10 0),(30 0,30 30,40 40))").toString(), f(g, c).toString());
};

new Emsg.Test("Emsg.Util.closest").run = function () {
  var wkt = OpenLayers.Geometry.fromWKT;
  var g = wkt("POINT(0 0)")
     ,f = Emsg.Util.closest;

  g = wkt("POINT(0 0)");
  f = Emsg.Util.closest;
  this.assertEqual(false, f(g, []));
  var a = wkt("POINT(0 10)")
    , b = wkt("POINT(0 20)")
    , c = wkt("POINT(0 30)")
    , d = wkt("POINT(0 0)")
    , e = wkt("POINT(0 0)");

  this.assertEqual(a, f(g, [a, b, c]));
  this.assertEqual(a, f(g, [b, a, c]));
  this.assertEqual(a, f(g, [b, c, a]));
  this.assertEqual(d, f(g, [a, b, c, d]));
  this.assertEqual(e, f(g, [a, e, b, c, d]));

  this.assertEqual(false, f(g, [b, c, a], 5));
  this.assertEqual(a, f(g, [b, c, a], 10));

  this.assertEqual(false, f(g, [b, c, a], 5, true));
  this.assertEqual(0, f(g, [a, b, c], undefined, true).index);
  this.assertEqual(1, f(g, [b, a, c], undefined, true).index);
  this.assertEqual(2, f(g, [b, c, a], undefined, true).index);

  this.assertEqual(0, f(g, [b, c, a], undefined, true).x0);
  this.assertEqual(0, f(g, [b, c, a], undefined, true).y0);
  this.assertEqual(0, f(g, [b, c, a], undefined, true).x1);
  this.assertEqual(10, f(g, [b, c, a], undefined, true).y1);
  this.assertEqual(10, f(g, [b, c, a], undefined, true).distance);
  this.assertEqual(a, f(g, [b, c, a], undefined, true).found);
};


new Emsg.Test("Emsg.Util.closestFeature").run = function () {
  var wkt = OpenLayers.Geometry.fromWKT;
  var f, g, F = OpenLayers.Feature.Vector;

  g = wkt("POINT(0 0)");
  f = Emsg.Util.closestFeature;
  this.assertEqual(false, f(g, []));
  var a = new F(wkt("POINT(0 10)"))
    , b = new F(wkt("POINT(0 20)"))
    , c = new F(wkt("POINT(0 30)"));

  this.assertEqual(a, f(g, [a, b, c]));
  this.assertEqual(a, f(g, [b, a, c]));
  this.assertEqual(a, f(g, [b, c, a]));

  this.assertEqual(false, f(g, [b, c, a], 5));
  this.assertEqual(a, f(g, [b, c, a], 10));

  this.assertEqual(false, f(g, [b, c, a], 5, true));
  this.assertEqual(0, f(g, [a, b, c], undefined, true).index);
  this.assertEqual(1, f(g, [b, a, c], undefined, true).index);
  this.assertEqual(2, f(g, [b, c, a], undefined, true).index);

  this.assertEqual(0, f(g, [b, c, a], undefined, true).x0);
  this.assertEqual(0, f(g, [b, c, a], undefined, true).y0);
  this.assertEqual(0, f(g, [b, c, a], undefined, true).x1);
  this.assertEqual(10, f(g, [b, c, a], undefined, true).y1);
  this.assertEqual(10, f(g, [b, c, a], undefined, true).distance);
  this.assertEqual(a, f(g, [b, c, a], undefined, true).found);
};


