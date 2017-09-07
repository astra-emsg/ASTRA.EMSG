<?xml version="1.0" encoding="UTF-8"?>
<StyledLayerDescriptor version="1.1.0"
    xsi:schemaLocation="http://www.opengis.net/sld StyledLayerDescriptor.xsd"
    xmlns="http://www.opengis.net/sld"
    xmlns:ogc="http://www.opengis.net/ogc"
    xmlns:xlink="http://www.w3.org/1999/xlink"
    xmlns:se="http://www.opengis.net/se" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
    <!-- a named layer is the basic building block of an sld document -->
  <NamedLayer>
    <se:Name>ZustandsabschnittStyle</se:Name>
    <UserStyle>
      <se:Name>ZustandsabschnittStyle</se:Name>
      <se:FeatureTypeStyle>

        <se:Rule>
          <se:Name>ZeroToOne</se:Name>
          <ogc:Filter>
         <ogc:And>
           <ogc:PropertyIsGreaterThanOrEqualTo>
             <ogc:PropertyName>ZST_INDEX</ogc:PropertyName>
             <ogc:Literal>0</ogc:Literal>
           </ogc:PropertyIsGreaterThanOrEqualTo>
           <ogc:PropertyIsLessThan>
             <ogc:PropertyName>ZST_INDEX</ogc:PropertyName>
             <ogc:Literal>1</ogc:Literal>
           </ogc:PropertyIsLessThan>
         </ogc:And>
       </ogc:Filter>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#1CCF34</se:SvgParameter>
              <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
              <se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
              <se:SvgParameter name="stroke-linecap">flat</se:SvgParameter>
            </se:Stroke>
          </se:LineSymbolizer>
          <se:PointSymbolizer>
            <se:Geometry>
              <ogc:Function name="startPoint">
                <ogc:PropertyName>SHAPE</ogc:PropertyName>
              </ogc:Function>
            </se:Geometry>
           <se:Graphic>
              <se:Mark>
                <se:WellKnownName>shape://vertline</se:WellKnownName>
                
                <se:Stroke>
                  <se:SvgParameter  name="stroke">#000000</se:SvgParameter >
                  <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
                </se:Stroke>
              </se:Mark>
              <se:Size>13</se:Size>
              <se:Rotation>
                  <ogc:Function name="startAngle">
                    <ogc:PropertyName>SHAPE</ogc:PropertyName>
                  </ogc:Function>
              </se:Rotation>
            </se:Graphic>
          </se:PointSymbolizer>
                    <se:PointSymbolizer>
            <se:Geometry>
              <ogc:Function name="endPoint">
                <ogc:PropertyName>SHAPE</ogc:PropertyName>
              </ogc:Function>
            </se:Geometry>
           <se:Graphic>
              <se:Mark>
                <se:WellKnownName>shape://vertline</se:WellKnownName>                 
                <se:Stroke>
                  <se:SvgParameter  name="stroke">#000000</se:SvgParameter >
                  <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
                </se:Stroke>
              </se:Mark>
              <se:Size>13</se:Size>
              <se:Rotation>                
                  <ogc:Function name="endAngle">
                    <ogc:PropertyName>SHAPE</ogc:PropertyName>
                  </ogc:Function>                 
              </se:Rotation>
            </se:Graphic>
          </se:PointSymbolizer>
        </se:Rule>       
        <se:Rule>
          <se:Name>OneToTwo</se:Name>
          <ogc:Filter>
         <ogc:And>
           <ogc:PropertyIsGreaterThanOrEqualTo>
             <ogc:PropertyName>ZST_INDEX</ogc:PropertyName>
             <ogc:Literal>1</ogc:Literal>
           </ogc:PropertyIsGreaterThanOrEqualTo>
           <ogc:PropertyIsLessThan>
             <ogc:PropertyName>ZST_INDEX</ogc:PropertyName>
             <ogc:Literal>2</ogc:Literal>
           </ogc:PropertyIsLessThan>
         </ogc:And>
       </ogc:Filter>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#F4F42D</se:SvgParameter>
              <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
              <se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
              <se:SvgParameter name="stroke-linecap">flat</se:SvgParameter>
            </se:Stroke>
          </se:LineSymbolizer>
          <se:PointSymbolizer>
            <se:Geometry>
              <ogc:Function name="startPoint">
                <ogc:PropertyName>SHAPE</ogc:PropertyName>
              </ogc:Function>
            </se:Geometry>
           <se:Graphic>
              <se:Mark>
                <se:WellKnownName>shape://vertline</se:WellKnownName>                 
                <se:Stroke>
                  <se:SvgParameter  name="stroke">#000000</se:SvgParameter >
                  <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
                </se:Stroke>
              </se:Mark>
              <se:Size>13</se:Size>
              <se:Rotation>
                  <ogc:Function name="startAngle">
                    <ogc:PropertyName>SHAPE</ogc:PropertyName>
                  </ogc:Function>
              </se:Rotation>
            </se:Graphic>
          </se:PointSymbolizer>
                    <se:PointSymbolizer>
            <se:Geometry>
              <ogc:Function name="endPoint">
                <ogc:PropertyName>SHAPE</ogc:PropertyName>
              </ogc:Function>
            </se:Geometry>
           <se:Graphic>
              <se:Mark>
                <se:WellKnownName>shape://vertline</se:WellKnownName>
                <se:Stroke>
                  <se:SvgParameter  name="stroke">#000000</se:SvgParameter >
                  <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
                </se:Stroke>
              </se:Mark>
              <se:Size>13</se:Size>
              <se:Rotation>                
                  <ogc:Function name="endAngle">
                    <ogc:PropertyName>SHAPE</ogc:PropertyName>
                  </ogc:Function>                 
              </se:Rotation>
            </se:Graphic>
          </se:PointSymbolizer>
        </se:Rule>
        <se:Rule>
          <se:Name>TwoToThree</se:Name>
          <ogc:Filter>
         <ogc:And>
           <ogc:PropertyIsGreaterThanOrEqualTo>
             <ogc:PropertyName>ZST_INDEX</ogc:PropertyName>
             <ogc:Literal>2</ogc:Literal>
           </ogc:PropertyIsGreaterThanOrEqualTo>
           <ogc:PropertyIsLessThan>
             <ogc:PropertyName>ZST_INDEX</ogc:PropertyName>
             <ogc:Literal>3</ogc:Literal>
           </ogc:PropertyIsLessThan>
         </ogc:And>
       </ogc:Filter>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#F2A232</se:SvgParameter>
              <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
              <se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
              <se:SvgParameter name="stroke-linecap">flat</se:SvgParameter>
            </se:Stroke>
          </se:LineSymbolizer>
          <se:PointSymbolizer>
            <se:Geometry>
              <ogc:Function name="startPoint">
                <ogc:PropertyName>SHAPE</ogc:PropertyName>
              </ogc:Function>
            </se:Geometry>
           <se:Graphic>
              <se:Mark>
                <se:WellKnownName>shape://vertline</se:WellKnownName>
                
                <se:Stroke>
                  <se:SvgParameter  name="stroke">#000000</se:SvgParameter >
                  <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
                </se:Stroke>
              </se:Mark>
              <se:Size>13</se:Size>
              <se:Rotation>
                  <ogc:Function name="startAngle">
                    <ogc:PropertyName>SHAPE</ogc:PropertyName>
                  </ogc:Function>
              </se:Rotation>
            </se:Graphic>
          </se:PointSymbolizer>
                    <se:PointSymbolizer>
            <se:Geometry>
              <ogc:Function name="endPoint">
                <ogc:PropertyName>SHAPE</ogc:PropertyName>
              </ogc:Function>
            </se:Geometry>
           <se:Graphic>
              <se:Mark>
                <se:WellKnownName>shape://vertline</se:WellKnownName>                 
                <se:Stroke>
                  <se:SvgParameter  name="stroke">#000000</se:SvgParameter >
                  <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
                </se:Stroke>
              </se:Mark>
              <se:Size>13</se:Size>
              <se:Rotation>                
                  <ogc:Function name="endAngle">
                    <ogc:PropertyName>SHAPE</ogc:PropertyName>
                  </ogc:Function>                 
              </se:Rotation>
            </se:Graphic>
          </se:PointSymbolizer>
        </se:Rule>       
        <se:Rule>
          <se:Name>ThreeToFour</se:Name>
          <ogc:Filter>
         <ogc:And>
           <ogc:PropertyIsGreaterThanOrEqualTo>
             <ogc:PropertyName>ZST_INDEX</ogc:PropertyName>
             <ogc:Literal>3</ogc:Literal>
           </ogc:PropertyIsGreaterThanOrEqualTo>
           <ogc:PropertyIsLessThan>
             <ogc:PropertyName>ZST_INDEX</ogc:PropertyName>
             <ogc:Literal>4</ogc:Literal>
           </ogc:PropertyIsLessThan>
         </ogc:And>
       </ogc:Filter>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#EF0000</se:SvgParameter>
              <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
              <se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
              <se:SvgParameter name="stroke-linecap">flat</se:SvgParameter>
            </se:Stroke>
          </se:LineSymbolizer>
          <se:PointSymbolizer>
            <se:Geometry>
              <ogc:Function name="startPoint">
                <ogc:PropertyName>SHAPE</ogc:PropertyName>
              </ogc:Function>
            </se:Geometry>
           <se:Graphic>
              <se:Mark>
                <se:WellKnownName>shape://vertline</se:WellKnownName>
                
                <se:Stroke>
                  <se:SvgParameter  name="stroke">#000000</se:SvgParameter >
                  <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
                </se:Stroke>
              </se:Mark>
              <se:Size>13</se:Size>
              <se:Rotation>
                  <ogc:Function name="startAngle">
                    <ogc:PropertyName>SHAPE</ogc:PropertyName>
                  </ogc:Function>
              </se:Rotation>
            </se:Graphic>
          </se:PointSymbolizer>
                    <se:PointSymbolizer>
            <se:Geometry>
              <ogc:Function name="endPoint">
                <ogc:PropertyName>SHAPE</ogc:PropertyName>
              </ogc:Function>
            </se:Geometry>
           <se:Graphic>
              <se:Mark>
                <se:WellKnownName>shape://vertline</se:WellKnownName>                 
                <se:Stroke>
                  <se:SvgParameter  name="stroke">#000000</se:SvgParameter >
                  <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
                </se:Stroke>
              </se:Mark>
              <se:Size>13</se:Size>
              <se:Rotation>                
                  <ogc:Function name="endAngle">
                    <ogc:PropertyName>SHAPE</ogc:PropertyName>
                  </ogc:Function>                 
              </se:Rotation>
            </se:Graphic>
          </se:PointSymbolizer>
        </se:Rule>       
        <se:Rule>
          <se:Name>FourToFive</se:Name>
          <ogc:Filter>
         <ogc:And>
           <ogc:PropertyIsGreaterThanOrEqualTo>
             <ogc:PropertyName>ZST_INDEX</ogc:PropertyName>
             <ogc:Literal>4</ogc:Literal>
           </ogc:PropertyIsGreaterThanOrEqualTo>
           <ogc:PropertyIsLessThanOrEqualTo>
             <ogc:PropertyName>ZST_INDEX</ogc:PropertyName>
             <ogc:Literal>5</ogc:Literal>
           </ogc:PropertyIsLessThanOrEqualTo>
         </ogc:And>
       </ogc:Filter>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#48024C</se:SvgParameter>
              <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
              <se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
              <se:SvgParameter name="stroke-linecap">flat</se:SvgParameter>
            </se:Stroke>
          </se:LineSymbolizer>
          <se:PointSymbolizer>
            <se:Geometry>
              <ogc:Function name="startPoint">
                <ogc:PropertyName>SHAPE</ogc:PropertyName>
              </ogc:Function>
            </se:Geometry>
           <se:Graphic>
              <se:Mark>
                <se:WellKnownName>shape://vertline</se:WellKnownName>
                
                <se:Stroke>
                  <se:SvgParameter  name="stroke">#000000</se:SvgParameter >
                  <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
                </se:Stroke>
              </se:Mark>
              <se:Size>13</se:Size>
              <se:Rotation>
                  <ogc:Function name="startAngle">
                    <ogc:PropertyName>SHAPE</ogc:PropertyName>
                  </ogc:Function>
              </se:Rotation>
            </se:Graphic>
          </se:PointSymbolizer>
                    <se:PointSymbolizer>
            <se:Geometry>
              <ogc:Function name="endPoint">
                <ogc:PropertyName>SHAPE</ogc:PropertyName>
              </ogc:Function>
            </se:Geometry>
           <se:Graphic>
              <se:Mark>
                <se:WellKnownName>shape://vertline</se:WellKnownName>                 
                <se:Stroke>
                  <se:SvgParameter  name="stroke">#000000</se:SvgParameter >
                  <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
                </se:Stroke>
              </se:Mark>
              <se:Size>13</se:Size>
              <se:Rotation>                
                  <ogc:Function name="endAngle">
                    <ogc:PropertyName>SHAPE</ogc:PropertyName>
                  </ogc:Function>                 
              </se:Rotation>
            </se:Graphic>
          </se:PointSymbolizer>
        </se:Rule>       
      </se:FeatureTypeStyle>
    </UserStyle>
  </NamedLayer>
</StyledLayerDescriptor>