<?xml version="1.0" encoding="UTF-8"?>
<StyledLayerDescriptor version="1.1.0" xsi:schemaLocation="http://www.opengis.net/sld StyledLayerDescriptor.xsd" xmlns="http://www.opengis.net/sld" xmlns:ogc="http://www.opengis.net/ogc" xmlns:xlink="http://www.w3.org/1999/xlink" xmlns:se="http://www.opengis.net/se" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<!-- a named layer is the basic building block of an sld document -->
	<NamedLayer>
		<se:Name>StrassenabschnittGISBK</se:Name>
		<UserStyle>
			<se:Name>StrassenabschnittGISBK</se:Name>
			<se:FeatureTypeStyle>
				<se:Rule>
					<se:Name>IA</se:Name>
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>0</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>IA</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#B7E9B1</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>1</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>IA</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#B7E9B1</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<se:Name>IB</se:Name>
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>0</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>IB</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#90D188</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>1</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>IB</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#90D188</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<se:Name>IC</se:Name>
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>0</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>IC</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#60AB57</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>1</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>IC</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#60AB57</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<se:Name>II</se:Name>
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>0</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>II</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#38AABB</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>1</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>II</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#38AABB</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<se:Name>III</se:Name>
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>0</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>III</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#5682BB</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>1</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>III</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#5682BB</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<se:Name>IA</se:Name>
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>0</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>IV</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#194EB3</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>1</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>IV</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#194EB3</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<se:Name>Pflaesterung</se:Name>
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>0</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>Pflaesterung</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#EEDFCC</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>1</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>Pflaesterung</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#EEDFCC</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<se:Name>Chaussierung</se:Name>
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>0</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>Chaussierung</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#CDC0B0</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>1</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>Chaussierung</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#CDC0B0</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<se:Name>Benutzerdefiniert1</se:Name>
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>0</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>Benutzerdefiniert1</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#8B8378</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>1</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>Benutzerdefiniert1</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#8B8378</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<se:Name>Benutzerdefiniert2</se:Name>
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>0</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>Benutzerdefiniert2</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#CDAF95</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>1</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>Benutzerdefiniert2</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#CDAF95</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<se:Name>Benutzerdefiniert3</se:Name>
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>0</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>Benutzerdefiniert3</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#8B7765</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<ogc:Filter>
						<ogc:And>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>ISINVERTED</ogc:PropertyName>
								<ogc:Literal>1</ogc:Literal>
							</ogc:PropertyIsEqualTo>
							<ogc:PropertyIsEqualTo>
								<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
								<ogc:Literal>Benutzerdefiniert3</ogc:Literal>
							</ogc:PropertyIsEqualTo>
						</ogc:And>
					</ogc:Filter>
					<se:MaxScaleDenominator>3000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#8B7765</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
									<se:SvgParameter name="fill">#9c9c9c</se:SvgParameter>
								</se:Fill>
								<se:Stroke>
									<se:SvgParameter name="stroke">#9c9c9c</se:SvgParameter>
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
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>IA</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>3000</se:MinScaleDenominator>
					<se:MaxScaleDenominator>16000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#B7E9B1</se:SvgParameter>
							<se:SvgParameter name="stroke-width">5</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>IB</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>3000</se:MinScaleDenominator>
					<se:MaxScaleDenominator>16000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#90D188</se:SvgParameter>
							<se:SvgParameter name="stroke-width">5</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>IC</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>3000</se:MinScaleDenominator>
					<se:MaxScaleDenominator>16000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#60AB57</se:SvgParameter>
							<se:SvgParameter name="stroke-width">5</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>II</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>3000</se:MinScaleDenominator>
					<se:MaxScaleDenominator>16000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#38AABB</se:SvgParameter>
							<se:SvgParameter name="stroke-width">5</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>III</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>3000</se:MinScaleDenominator>
					<se:MaxScaleDenominator>16000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#5682BB</se:SvgParameter>
							<se:SvgParameter name="stroke-width">5</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>IV</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>3000</se:MinScaleDenominator>
					<se:MaxScaleDenominator>16000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#194EB3</se:SvgParameter>
							<se:SvgParameter name="stroke-width">5</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>Pflaesterung</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>3000</se:MinScaleDenominator>
					<se:MaxScaleDenominator>16000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#EEDFCC</se:SvgParameter>
							<se:SvgParameter name="stroke-width">5</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>Chaussierung</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>3000</se:MinScaleDenominator>
					<se:MaxScaleDenominator>16000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#CDC0B0</se:SvgParameter>
							<se:SvgParameter name="stroke-width">5</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>Benutzerdefiniert1</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>3000</se:MinScaleDenominator>
					<se:MaxScaleDenominator>16000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#8B8378</se:SvgParameter>
							<se:SvgParameter name="stroke-width">5</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>Benutzerdefiniert2</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>3000</se:MinScaleDenominator>
					<se:MaxScaleDenominator>16000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#CDAF95</se:SvgParameter>
							<se:SvgParameter name="stroke-width">5</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>Benutzerdefiniert3</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>3000</se:MinScaleDenominator>
					<se:MaxScaleDenominator>16000</se:MaxScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#8B7765</se:SvgParameter>
							<se:SvgParameter name="stroke-width">5</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>IA</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>16000</se:MinScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#B7E9B1</se:SvgParameter>
							<se:SvgParameter name="stroke-width">1.9</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>IB</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>16000</se:MinScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#90D188</se:SvgParameter>
							<se:SvgParameter name="stroke-width">1.9</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>IC</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>16000</se:MinScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#60AB57</se:SvgParameter>
							<se:SvgParameter name="stroke-width">1.9</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>II</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>16000</se:MinScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#38AABB</se:SvgParameter>
							<se:SvgParameter name="stroke-width">1.9</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>III</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>16000</se:MinScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#5682BB</se:SvgParameter>
							<se:SvgParameter name="stroke-width">1.9</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>IV</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>16000</se:MinScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#194EB3</se:SvgParameter>
							<se:SvgParameter name="stroke-width">1.9</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>Pflaesterung</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>16000</se:MinScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#EEDFCC</se:SvgParameter>
							<se:SvgParameter name="stroke-width">1.9</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>Chaussierung</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>16000</se:MinScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#CDC0B0</se:SvgParameter>
							<se:SvgParameter name="stroke-width">1.9</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>Benutzerdefiniert1</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>16000</se:MinScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#8B8378</se:SvgParameter>
							<se:SvgParameter name="stroke-width">1.9</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>Benutzerdefiniert2</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>16000</se:MinScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#CDAF95</se:SvgParameter>
							<se:SvgParameter name="stroke-width">1.9</se:SvgParameter>
							<se:SvgParameter name="stroke-linejoin">bevel</se:SvgParameter>
							<se:SvgParameter name="stroke-linecap">round</se:SvgParameter>
						</se:Stroke>
					</se:LineSymbolizer>
				</se:Rule>
				<se:Rule>
					<ogc:Filter>
						<ogc:PropertyIsEqualTo>
							<ogc:PropertyName>blk_typ_vl</ogc:PropertyName>
							<ogc:Literal>Benutzerdefiniert3</ogc:Literal>
						</ogc:PropertyIsEqualTo>
					</ogc:Filter>
					<se:MinScaleDenominator>16000</se:MinScaleDenominator>
					<se:LineSymbolizer>
						<se:Stroke>
							<se:SvgParameter name="stroke">#8B7765</se:SvgParameter>
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