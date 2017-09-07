The Report uses a custom not UI supported ChartSeriesHierarchy.
In VS2012 the designer tries to fix the XML so it messes up the grupping, so you need to fix it by hand
after you finished working with the designer.

The current the correct ChartSeriesHierarchy looks like this:

     <ChartSeriesHierarchy>
          <ChartMembers>
            <ChartMember>
              <Group Name="Chart1_SeriesGroup1">
                <GroupExpressions>
                  <GroupExpression>=Fields!BelastungskategorieBezeichnung.Value</GroupExpression>
                </GroupExpressions>
              </Group>
              <ChartMembers>
                <ChartMember>
                  <Label>Flaeche Fahrbahn</Label>
                </ChartMember>
              </ChartMembers>
              <Label>=Fields!BelastungskategorieBezeichnung.Value</Label>
            </ChartMember>
                <ChartMember>
                  <Label>Wieder Beschaffungs Wert</Label>
                </ChartMember>
                <ChartMember>
                  <Label>Wert Verlust</Label>
            </ChartMember>
          </ChartMembers>
        </ChartSeriesHierarchy>