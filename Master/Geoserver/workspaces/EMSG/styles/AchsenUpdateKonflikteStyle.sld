<?xml version="1.0" encoding="UTF-8"?>
<StyledLayerDescriptor version="1.1.0"
    xsi:schemaLocation="http://www.opengis.net/sld StyledLayerDescriptor.xsd"
    xmlns="http://www.opengis.net/sld"
    xmlns:ogc="http://www.opengis.net/ogc"
    xmlns:xlink="http://www.w3.org/1999/xlink"
    xmlns:se="http://www.opengis.net/se" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
    <!-- a named layer is the basic building block of an sld document -->
  <NamedLayer>
    <se:Name>AchsenUpdateKonflikteStyle</se:Name>
    <UserStyle>
      <se:Name>AchsenUpdateKonflikteStyle</se:Name>
      <se:FeatureTypeStyle>

        <se:Rule>
          <se:Name>ConflictType_NEWSEGMENT</se:Name>
          <ogc:Filter>

           <ogc:PropertyIsEqualTo>
             <ogc:PropertyName>ConflictType</ogc:PropertyName>
             <ogc:Literal>1</ogc:Literal>
           </ogc:PropertyIsEqualTo>

       </ogc:Filter>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#38A800</se:SvgParameter>
              <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
              <se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
              <se:SvgParameter name="stroke-linecap">flat</se:SvgParameter>
            </se:Stroke>
          </se:LineSymbolizer>
        </se:Rule>
        <se:Rule>
          <se:Name>ConflictType_PARTIAL_SEGMENT_DELETED</se:Name>
          <ogc:Filter>

           <ogc:PropertyIsEqualTo>
             <ogc:PropertyName>ConflictType</ogc:PropertyName>
             <ogc:Literal>2</ogc:Literal>
           </ogc:PropertyIsEqualTo>

       </ogc:Filter>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#6FC400</se:SvgParameter>
              <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
              <se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
              <se:SvgParameter name="stroke-linecap">flat</se:SvgParameter>
            </se:Stroke>
          </se:LineSymbolizer>
        </se:Rule>
         <se:Rule>
          <se:Name>ConflictType_PARTIAL_REFERENCE_OUTSIDE</se:Name>
          <ogc:Filter>

           <ogc:PropertyIsEqualTo>
             <ogc:PropertyName>ConflictType</ogc:PropertyName>
             <ogc:Literal>3</ogc:Literal>
           </ogc:PropertyIsEqualTo>

       </ogc:Filter>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#B0E000</se:SvgParameter>
              <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
              <se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
              <se:SvgParameter name="stroke-linecap">flat</se:SvgParameter>
            </se:Stroke>
          </se:LineSymbolizer>
        </se:Rule>
        <se:Rule>
          <se:Name>ConflictType_PARTIAL_REFERENCE_TOOSHORT</se:Name>
          <ogc:Filter>

           <ogc:PropertyIsEqualTo>
             <ogc:PropertyName>ConflictType</ogc:PropertyName>
             <ogc:Literal>4</ogc:Literal>
           </ogc:PropertyIsEqualTo>

       </ogc:Filter>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#FFFF00</se:SvgParameter>
              <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
              <se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
              <se:SvgParameter name="stroke-linecap">flat</se:SvgParameter>
            </se:Stroke>
          </se:LineSymbolizer>
        </se:Rule>
		 <se:Rule>
          <se:Name>ConflictType_COMPLETELOSS_SEGMENT_DELETED</se:Name>
          <ogc:Filter>

           <ogc:PropertyIsEqualTo>
             <ogc:PropertyName>ConflictType</ogc:PropertyName>
             <ogc:Literal>18</ogc:Literal>
           </ogc:PropertyIsEqualTo>

       </ogc:Filter>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#FFAA00</se:SvgParameter>
              <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
              <se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
              <se:SvgParameter name="stroke-linecap">flat</se:SvgParameter>
            </se:Stroke>
          </se:LineSymbolizer>
        </se:Rule>
                <se:Rule>
          <se:Name>ConflictType_COMPLETELOSS_REFERENCE_OUTSIDE</se:Name>
          <ogc:Filter>

           <ogc:PropertyIsEqualTo>
             <ogc:PropertyName>ConflictType</ogc:PropertyName>
             <ogc:Literal>19</ogc:Literal>
           </ogc:PropertyIsEqualTo>

       </ogc:Filter>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#FF5500</se:SvgParameter>
              <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
              <se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
              <se:SvgParameter name="stroke-linecap">flat</se:SvgParameter>
            </se:Stroke>
          </se:LineSymbolizer>
        </se:Rule>
       
        <se:Rule>
          <se:Name>ConflictType_COMPLETELOSS_REFERENCE_TOOSHORT</se:Name>
          <ogc:Filter>

           <ogc:PropertyIsEqualTo>
             <ogc:PropertyName>ConflictType</ogc:PropertyName>
             <ogc:Literal>20</ogc:Literal>
           </ogc:PropertyIsEqualTo>

       </ogc:Filter>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#FF0000</se:SvgParameter>
              <se:SvgParameter name="stroke-width">2.5</se:SvgParameter>
              <se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
              <se:SvgParameter name="stroke-linecap">flat</se:SvgParameter>
            </se:Stroke>
          </se:LineSymbolizer>
        </se:Rule>
        </se:FeatureTypeStyle>
    </UserStyle>
  </NamedLayer>
</StyledLayerDescriptor>