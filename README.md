# MatrixViewer / MatrixEditor

##  例

```
    <local:MatrixViewer
        Grid.Column="0"  Grid.Row="0"
        MatrixData="{Binding MatrixData}"
        Rows="{Binding TotalRows}"
        Columns="{Binding TotalColumns}"
        HorizontalScrollBarVisibility="Auto"
        VerticalScrollBarVisibility="Auto"
    >
      <local:MatrixViewer.Layouts>
        <local:MatrixLayout
          DefaultCellWidth="72.0"
          DefaultCellHeight="25.0"
        >
        </local:MatrixLayout>
      </local:MatrixViewer.Layouts>
    </local:MatrixViewer>
```
