   M S B A M L   `   `   `  ����   FrontV28    2FrontV2.TauxCredit.Indicateurs.View.IndicateurView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     l hTelerik.Windows.Documents.Core, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7x tTelerik.Windows.Controls.DataVisualization, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7g cTelerik.Windows.Documents, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.RibbonView, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7c	 _Telerik.Windows.Maths, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o
 kTelerik.Windows.Controls.GridView, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7| xTelerik.Windows.Documents.FormatProviders.Xaml, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7| xTelerik.Windows.Documents.FormatProviders.Html, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r nTelerik.Windows.Controls.ImageEditor, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7 {Telerik.Windows.Documents.FormatProviders.OpenXml, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7{ wTelerik.Windows.Documents.FormatProviders.Rtf, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Input, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7f bTelerik.Windows.Controls, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7t pTelerik.Windows.Controls.RichTextBoxUI, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Chart, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7_telerik1http://schemas.telerik.com/2008/xaml/presentation     	 
              � Title$  IndicateurView��$
��1270��$	��650�� � WindowState$ 	Maximized=�.��� $��1,0,0,0q�$
��Left=�$
��1243��.+��� $	��Top=�$��0,31,609.134,0q� �� Orientation$ 
Horizontal=�.+��� $
��ISIN��$��Arial Black)�1  +Telerik.Windows.Controls.RadAutoCompleteBox  $��589.667��$��5,0,0,0q�   ItemsSource �� *Isin+   SelectedItems �� *SelectedIsins+ �� Mode$ TwoWay=��� $��Right=�$	��Top=�$��0,31,-51.5,0q�$ 
Horizontal=�$	��659��.+��� $��Source��$��0q�$��Arial Black)��� $	��120������ *	Source1+ �� SelectedValue �� *SelectedSource+$ TwoWay=��� -    ��Name$ Date_F��$��Depart Date��$��Arial Black)�$��15,0,0,0q�  $	��125�� �� *DateD+ �� *SelectedDateD+$ TwoWay=��� $��
Final Date��$��10,0,0,0q�$��Arial Black)�  $	��120�� �� *DateF+ �� *SelectedDateF+$ TwoWay=��� $
��Left=�$	��Top=�$��	0,105,0,0q�$��1141.91��.+��� $	��Top=�$��0,0,-114.5,0q�.+�* 
 $Telerik.Windows.Controls.RadGridView  -     Name$ RadGrid��; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl	   ClipboardCopyMode$		 All=�
   CanUserDeleteRows
 .     CanUserInsertRows .     
IsReadOnly .    AlternateRowBackground
 �����8 
 2Telerik.Windows.Controls.GridView.BaseItemsControl   AlternationCount$ 2��   SelectionMode$ Extended=�   ClipboardPasteMode$ AllSelectedCells=�   AutoGenerateColumns .    SelectionUnit$ FullRow=�*  $Telerik.Windows.Controls.DataControl$   IsSynchronizedWithCurrentItem$
 TrueY�$��10,0,84.205,0q�   ShowGroupPanel .     ItemsSource �� *IndicateursDataSource+�� -   $
��SHOW��$��
1189,0,0,0q�$��23��$��Arial Black)�����  �� EndPoint$	 0,1&� �� 
StartPoint$	 0,0&�.���  � Color$ 	#FFF3F3F3�� � Offset$ 0Q�� $ 	#FFEBEBEB��$	 0.5Q�� $ 	#FFDDDDDD��$	 0.5Q�� $ 	#FFEAB52A��$ 1Q��� -   $��Export��$��1189,-75,0,6q� J� RenderTransformOrigin$ 1.241,0.638&�$��1,2,1,1q�$��27��$��Arial Black)����� $	 0,1&�$	 0,0&�.��� $ 	#FFF3F3F3��$ 0Q�� $ 	#FFEBEBEB��$	 0.5Q�� $ 	#FFDDDDDD��$ 0.592Q�� $ 	#FF190303��$ 1Q��� $ 
Horizontal=�$��	0,15,10,0q�.+��� $��Indicator :��$��Arial Black)�$��13.333(��� $��Closing Price��$��40,0,0,0q����	#FF16F5E1$��40q� d� 	IsChecked �� *IsSelected11+�� $��Mobile Average��$��	34,0,15,0q�$��108.413��$ 
0.511,0.47&����	#FF27A60B$��40q� �� *IsSelected1+z�V� .~��� �� ��  �� Angle$ -0.137Q�T� �� $��Historical Average��$��5,0,0,0q�
����  �$��40q� �� *IsSelected2+�� $��Average��$��24,0,0,0q����	#FFBD1650$��40q� �� *IsSelected12+�� $��
Volatility��$��	44,0,10,0q����	#FFF7230C$��40q� �� *IsSelected3+�� $��Mobile Volatility��$��	51,0,15,0q����	#FFED7F10$��40q� �� *IsSelected4+�� $�� Z Score��$��10,0,0,0q�$��0q����	#FFB770DE$��40q� �� *IsSelected5+�� $ 
Horizontal=�$��0,0,10,0q�.+��� $��Mobile Z Score��$��
125,0,10,0q����	#FF664227$��40q� �� *IsSelected6+�� $��Maximum��$��12,0,0,0q����	#FFF18D27$��40q� �� *IsSelected7+�� $��Maximum  5%��$��59,0,0,0q�
��� ���$��40q� �� *IsSelected8+�� $��Minimum��$��44,0,0,0q����	#FF643205$��40q� �� *IsSelected9+�� $��
Minimum 5%��$��36,0,0,0q����	#FFF9429E$��40q� �� *IsSelected10+�� -   $��
Show Graph��$	��109��$��	356,0,0,0q�$��Arial Black)�$��13.333(����� $	 0,1&�$	 0,0&�.��� $ 	#FFF3F3F3��$ 0Q�� $ 	#FFEBEBEB��$	 0.5Q�� $ 	#FFDDDDDD��$ 0.619Q�� $ 	#FF190303��$ 1Q��� $��	20,80,0,0q�$	��313��$ 
Horizontal=�.+�0  *Telerik.Windows.Controls.RadCartesianChart  -     Name$ telerik��$	��313��$��0q�$��1121.5��   HorizontalAxis 8  2Telerik.Windows.Controls.ChartView.CategoricalAxis  -  'Telerik.Windows.Controls.ChartView.Axis   LabelFitMode$ Rotate=�    LabelRotationAngle$  80Q�!   VerticalAxis! 3	  -Telerik.Windows.Controls.ChartView.LinearAxis	  5
  /Telerik.Windows.Controls.ChartView.RadChartBase" 
  	Behaviors" @  :Telerik.Windows.Controls.ChartView.ChartPanAndZoomBehavior  #   ZoomMode$# 
Horizontal=�$   Series.$ 3  -Telerik.Windows.Controls.ChartView.LineSeries  -   %  Name$% 
LineChart1��A  ;Telerik.Windows.Controls.ChartView.CategoricalStrokedSeries&   Stroke
& � � �:  4Telerik.Windows.Controls.ChartView.CategoricalSeries'   CategoryBinding' �� *CategoriesSelector+(   ValueBinding( �� *ValuesSelector+4  .Telerik.Windows.Controls.ChartView.ChartSeries)   ItemsSource) �� *
DataList+  -   $% 
LineChart2��
& ��  �' �� *CategoriesSelector+( �� *ValuesSelector+) �� *	DataList2+���� $ 0.5,1&�$ 0.5,0&�.��� $ Black��$ 0Q�� $ White��$ 1Q�  -	   $% 
LineChart3��
& �  ��' �� *CategoriesSelector+( �� *ValuesSelector+) �� *	DataList3+  -
   $% 
LineChart4��
& � ���' �� *CategoriesSelector+( �� *ValuesSelector+) �� *	DataList4+  -   $% 
LineChart5��
& ����' �� *CategoriesSelector+( �� *ValuesSelector+) �� *	DataList5+  -   $% 
LineChart6��
& �i��' �� *CategoriesSelector+( �� *ValuesSelector+) �� *	DataList6+  -   $% 
LineChart7��
& �P��' �� *CategoriesSelector+( �� *ValuesSelector+) �� *	DataList7+  -   $% 
LineChart8��
& � ���' �� *CategoriesSelector+( �� *ValuesSelector+) �� *	DataList8+  -   $% 
LineChart9��
& �**��' �� *CategoriesSelector+( �� *ValuesSelector+) �� *	DataList9+  -   $% LineChart10��
& ����' �� *CategoriesSelector+( �� *ValuesSelector+) �� *
DataList10+  -   $% LineChart11��
& ��� �' �� *CategoriesSelector+( �� *ValuesSelector+) �� *
DataList11+  -   $% LineChart12��
& �� ��' �� *CategoriesSelector+( �� *ValuesSelector+) �� *
DataList12+  -   $% ZeroLine��
& �   �*   StrokeThickness$* 1Q�' �� *CategoriesSelector+( �� *ValuesSelector+) �� *
ZeroLine+