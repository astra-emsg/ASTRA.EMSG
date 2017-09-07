<?xml version="1.0" encoding="UTF-8"?>
<StyledLayerDescriptor version="1.1.0" 
    xsi:schemaLocation="http://www.opengis.net/sld StyledLayerDescriptor.xsd" 
    xmlns="http://www.opengis.net/sld" 
    xmlns:ogc="http://www.opengis.net/ogc" 
    xmlns:xlink="http://www.w3.org/1999/xlink" 
    xmlns:se="http://www.opengis.net/se" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
    <!-- a named layer is the basic building block of an sld document -->

  <NamedLayer>
    <se:Name>KoordinierteMassnahmenStyle</se:Name>
    <UserStyle>       
      <se:FeatureTypeStyle>
       
        <se:Rule>
          <se:Name>KoordinierteMassnahmen</se:Name>
          <se:LineSymbolizer>
            <se:Stroke>
              <se:SvgParameter name="stroke">#30A100</se:SvgParameter>
              <se:SvgParameter name="stroke-width">11.9</se:SvgParameter>
            </se:Stroke>
          </se:LineSymbolizer>
        </se:Rule>

        </se:FeatureTypeStyle>
    </UserStyle>
  </NamedLayer>
</StyledLayerDescriptor>