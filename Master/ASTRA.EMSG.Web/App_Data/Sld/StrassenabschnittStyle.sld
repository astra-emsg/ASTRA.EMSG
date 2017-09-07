<?xml version="1.0" encoding="UTF-8"?>
<StyledLayerDescriptor version="1.1.0" xsi:schemaLocation="http://www.opengis.net/sld StyledLayerDescriptor.xsd" xmlns="http://www.opengis.net/sld" xmlns:ogc="http://www.opengis.net/ogc" xmlns:xlink="http://www.w3.org/1999/xlink" xmlns:se="http://www.opengis.net/se" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <!-- a named layer is the basic building block of an sld document -->
  <NamedLayer>
    <se:Name>StrassenabschnittStyle</se:Name>
    <UserStyle>
      <se:Name>StrassenabschnittStyle</se:Name>
      <se:FeatureTypeStyle>
        <se:Rule>
          <se:Name>Strassenabschnitte</se:Name>
          <ogc:Filter>						
            <ogc:PropertyIsEqualTo>
              <ogc:PropertyName>ISINVERTED</ogc:PropertyName>
              <ogc:Literal>0</ogc:Literal>
            </ogc:PropertyIsEqualTo>						
          </ogc:Filter>
          <se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#9C9C9C</se:SvgParameter>
              <se:SvgParameter name="stroke-width">10</se:SvgParameter>
              <se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
              <se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
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
                <se:WellKnownName>triangle</se:WellKnownName>
                <se:Fill>
                  <se:SvgParameter name="fill">#343434</se:SvgParameter>
                </se:Fill>
                <se:Stroke>
                  <se:SvgParameter name="stroke">#343434</se:SvgParameter>
                </se:Stroke>
              </se:Mark>
              <se:Size>10</se:Size>
              <se:Rotation>
                <ogc:Add>
                  <ogc:Function name="startAngle">
                    <ogc:PropertyName>SHAPE</ogc:PropertyName>
                  </ogc:Function>
                  <ogc:Literal>90.0</ogc:Literal>
                </ogc:Add>
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
                <se:WellKnownName>triangle</se:WellKnownName>
                <se:Fill>
                  <se:SvgParameter name="fill">#343434</se:SvgParameter>
                </se:Fill>
                <se:Stroke>
                  <se:SvgParameter name="stroke">#343434</se:SvgParameter>
                </se:Stroke>
              </se:Mark>
              <se:Size>10</se:Size>
              <se:Rotation>
                <ogc:Add>
                  <ogc:Function name="endAngle">
                    <ogc:PropertyName>SHAPE</ogc:PropertyName>
                  </ogc:Function>
                  <ogc:Literal>90.0</ogc:Literal>
                </ogc:Add>
              </se:Rotation>
            </se:Graphic>
          </se:PointSymbolizer>
        </se:Rule>
        <se:Rule>
              <ogc:PropertyIsEqualTo>
                <ogc:PropertyName>ISINVERTED</ogc:PropertyName>
                <ogc:Literal>1</ogc:Literal>
              </ogc:PropertyIsEqualTo>              
          <se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#9C9C9C</se:SvgParameter>
              <se:SvgParameter name="stroke-width">10</se:SvgParameter>
              <se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
              <se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
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
                <se:WellKnownName>triangle</se:WellKnownName>
                <se:Fill>
                  <se:SvgParameter name="fill">#343434</se:SvgParameter>
                </se:Fill>
                <se:Stroke>
                  <se:SvgParameter name="stroke">#343434</se:SvgParameter>
                </se:Stroke>
              </se:Mark>
              <se:Size>10</se:Size>
              <se:Rotation>
                <ogc:Add>
                  <ogc:Function name="startAngle">
                    <ogc:PropertyName>SHAPE</ogc:PropertyName>
                  </ogc:Function>
                  <ogc:Literal>270</ogc:Literal>
                </ogc:Add>
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
                <se:WellKnownName>triangle</se:WellKnownName>
                <se:Fill>
                  <se:SvgParameter name="fill">#343434</se:SvgParameter>
                </se:Fill>
                <se:Stroke>
                  <se:SvgParameter name="stroke">#343434</se:SvgParameter>
                </se:Stroke>
              </se:Mark>
              <se:Size>10</se:Size>
              <se:Rotation>
                <ogc:Add>
                  <ogc:Function name="endAngle">
                    <ogc:PropertyName>SHAPE</ogc:PropertyName>
                  </ogc:Function>
                  <ogc:Literal>270</ogc:Literal>
                </ogc:Add>
              </se:Rotation>
            </se:Graphic>
          </se:PointSymbolizer>
        </se:Rule>				
        <se:Rule>
          <se:MinScaleDenominator>3000</se:MinScaleDenominator>
          <se:MaxScaleDenominator>16000</se:MaxScaleDenominator>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#9C9C9C</se:SvgParameter>
              <se:SvgParameter name="stroke-width">5</se:SvgParameter>
              <se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
              <se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
            </se:Stroke>
          </se:LineSymbolizer>
        </se:Rule>				
        <se:Rule>
          <se:MinScaleDenominator>16000</se:MinScaleDenominator>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#9C9C9C</se:SvgParameter>
              <se:SvgParameter name="stroke-width">1.9</se:SvgParameter>
              <se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
              <se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
            </se:Stroke>
          </se:LineSymbolizer>
        </se:Rule>
      </se:FeatureTypeStyle>
    </UserStyle>
  </NamedLayer>
</StyledLayerDescriptor>