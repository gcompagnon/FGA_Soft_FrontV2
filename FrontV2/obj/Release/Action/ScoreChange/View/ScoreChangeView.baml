   M S B A M L   `   `   `  ����   FrontV25    /FrontV2.Action.ScoreChange.View.ScoreChangeView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     l hTelerik.Windows.Documents.Core, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7x tTelerik.Windows.Controls.DataVisualization, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7g cTelerik.Windows.Documents, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.RibbonView, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7c	 _Telerik.Windows.Maths, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o
 kTelerik.Windows.Controls.GridView, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7| xTelerik.Windows.Documents.FormatProviders.Xaml, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7| xTelerik.Windows.Documents.FormatProviders.Html, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r nTelerik.Windows.Controls.ImageEditor, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7 {Telerik.Windows.Documents.FormatProviders.OpenXml, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7{ wTelerik.Windows.Documents.FormatProviders.Rtf, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Input, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7f bTelerik.Windows.Controls, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7t pTelerik.Windows.Controls.RichTextBoxUI, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Chart, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7_telerik1http://schemas.telerik.com/2008/xaml/presentation     	 
            Zd2http://schemas.microsoft.com/expression/blend/2008     	 
            dmc;http://schemas.openxmlformats.org/markup-compatibility/2006     	 
            -     �Name$  window�� � Icon$ /fga.ico�� � Title$ Changements de scores��$
��1017��$	��731�� � WindowState$ 	Maximized=� � 	Resources  )  %GridViewGroupPanelStyleExpandCollapse �� 
TargetType: 
 4Telerik.Windows.Controls.GridView.GridViewGroupPanel �� Property �� Value �� 
TargetType	 � UseLayoutRounding'  !System.Windows.VisualStateManager
   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard �� TargetProperty �� 
TargetName R� KeyTime R� Value  System.Windows.Visibility �� Orientation ��Name J� IsHitTestVisible2  ,Telerik.Windows.Controls.LocalizationManager   ResourceKey K� IsItemsHost �� EndPoint �� 
StartPoint � Color � Offset��%"  &	        �� "  .���  � �� �� "  .R�� 	 . 
   Grouping��.   NoGroups��  	HasGroups��. �� .��T� $ 
Visibility�$ 	panelText��..�i� $ 0��  	Collapsed=�T� $ 
Visibility�$ panelTextGrouped��..�i� $ 0��  Visible=�.+��� #��z��#��z��._��� ���	#FFEFF6FF$��1q�#��z���� $��0,0,8,0q�$ 
Horizontal=�.+��� -   $��2q�$��
expand.png���� -   $��2q�$��collapse.png���� $ 	panelText��$	��9.5(�#��z�� .  #��z��$ GridViewGroupPanelText��$b��\Faire glisser l'en-tête de la colonne et le déposer près de la colonne pour les regrouper��#��z���� $b�\Faire glisser l'en-tête de la colonne et le déposer près de la colonne pour les regrouper���� $ panelTextGrouped��#��z�� .  #��z��$&  GridViewGroupPanelTopTextGrouped��$��Regroupé par��$y�	Collapsed=�#��z���� $�Regroupé par���� $��Center=���  � �� �� .R���  . $ 
Horizontal=�$��Stretch=���  � ��$ 32����  � ��$ 0q���  � ��$ 10,0,3,0q���  � �� �� $ 0.5,1&�$ 0.5,0&�.��� $ 	#FFDFDFDF��$ 1Q�� $ 	#FFBABABA����  � �� �	#FF848484��  � ��$ 1q���  � ��$ Center=���  � ��$
 Left=���  �@J�SnapsToDevicePixels . .��� .+��� $��Filtres��$
��Left=�$	��Top=�$��	10,10,0,0q�$	��315��$	��145��.��� $��0q�.+��� $��	30,19,0,0q�$	��Top=�$
��Left=�$ 
Horizontal=�.+��� $
��Wrap=�$��Center=�$��22��$��Date 1 (Récente)��$��95���� $	��5,0q�$��Center=����� *Dates+ �� SelectedItem �� *SelectedDate1+ �� Mode$ TwoWay=��� $��	30,46,0,0q�$
��Left=�$	��Top=�$ 
Horizontal=�.+��� $
��Wrap=�$��Date 2 (Ancienne)��$��Center=�$��22��$��95���� $��Center=�$	��5,0q����� *Dates+ �� *SelectedDate2+$ TwoWay=�(  "Telerik.Windows.Controls.RadButton  -   $��	 Charger ��$��205.41,86.08,0,0q�$
��Left=�$	��Top=�$��25.96��$��58.59���� -   $��Filter1��$$��Écarts rangs inférieurs à 1��$��319,108.167,-231.15,-6q��� -   $��Exporter PDF��$��99.712,87.04,114,10q� J� RenderTransformOrigin$ -0.154,3.557&�$��Center=�$��Center=�$��25��$��89.288��* 
 $Telerik.Windows.Controls.RadGridView  -     Name$ ChangesGrid��;	 
 5Telerik.Windows.Controls.GridView.GridViewDataControl 	  ClipboardCopyMode$	 All=� 	  CanUserDeleteRows .   	  CanUserInsertRows  .  ! 	  
IsReadOnly! . " 	  AlternateRowBackground
" �����8
 
 2Telerik.Windows.Controls.GridView.BaseItemsControl# 
  AlternationCount$# 2��$ 	  SelectionMode$$ Extended=�% 	  ClipboardPasteMode$% AllSelectedCells=�& 	  AutoGenerateColumns& . ' 	  SelectionUnit$' FullRow=�*  $Telerik.Windows.Controls.DataControl$(   IsSynchronizedWithCurrentItem$
( TrueY�$��555,10,10,10q�!) 	  EnableColumnVirtualization) .  * 	  EnableRowVirtualization* .  + 	  GroupPanelStyle#+ �   ,   ItemsSource, �� *ChangesDataSource+- 	  SortDescriptors- )  #Telerik.Windows.Data.SortDescriptor  .   Member$. SECTOR��-  'Telerik.Windows.Data.SortDescriptorBase/   SortDirection$/ 	Ascending=�  $. INDUSTRY��$/ 	Ascending=�-  'Telerik.Windows.Controls.RadContextMenu0   ContextMenu0   -   .@�*  $Telerik.Windows.Controls.RadMenuItem  -	   $+��%Montrer l'évolution sur le graphique��1   IconTemplate1 �� .R��� $)��#/FrontV2;component/Images/graph.png��$��UniformToFill=�  $��Fiches Valeurs��.@�  -
   $��Ouvrir fiche excel��1 �� .R��� $)��#/FrontV2;component/Images/excel.png��$��UniformToFill=�  -   $��Ouvrir fiche pdf��1 �� .R��� $'��!/FrontV2;component/Images/pdf.png��$��UniformToFill=�  $��Liens Bloomberg��.@�  -   $	��DES��  -   $��BQ��  -   $
��HCPI��  -   $	��WGT��  -   $��GR��3  -Telerik.Windows.Controls.RadMenuSeparatorItem    -   $	��BRC��  -   $	��EVT��  -   $	��ICN��  -   $	��EEG��    -   $
��EQRV��  -   $
��PBAR��  -   $	��ANR��    -   $��GIP 5��  -   $��GIP 30���� $��10,160,0,10q�$	��540��$
��Left=�.+�0  *Telerik.Windows.Controls.RadCartesianChart  -   2  Name$2 quintilesChart��$
��10,0q�$	��300��0   .@�  -   $��Copier dans Paint��5  /Telerik.Windows.Controls.ChartView.RadChartBase3   	Behaviors3 ?  9Telerik.Windows.Controls.ChartView.ChartSelectionBehavior  -   4   DataPointSelectionMode$4 Single=�@  :Telerik.Windows.Controls.ChartView.ChartPanAndZoomBehavior  5   ZoomMode$5 
Horizontal=�6   PanMode$6 
Horizontal=�7   HorizontalAxis7 8  2Telerik.Windows.Controls.ChartView.CategoricalAxis  -  'Telerik.Windows.Controls.ChartView.Axis8   LabelFitMode$8 Rotate=�9   LabelRotationAngle$9 90Q�:   LabelInterval: �� *
Interval+;   VerticalAxis; 3  -Telerik.Windows.Controls.ChartView.LinearAxis  6  0Telerik.Windows.Controls.ChartView.NumericalAxis<   MaximumA  ;Telerik.Windows.Controls.Converters.StringToDoubleConverter$< 6 =   Minimum$= 0 1  +Telerik.Windows.Controls.ChartView.LineAxis>   	IsInverse> . ?   	MajorStep$? 1 @   Series.@ 3  -Telerik.Windows.Controls.ChartView.LineSeries  A  ;Telerik.Windows.Controls.ChartView.CategoricalStrokedSeriesA   Stroke
A � � �:  4Telerik.Windows.Controls.ChartView.CategoricalSeriesB   CategoryBindingB �� *CategoriesSelectorQuint+C   ValueBindingC �� *ValuesSelectorQuint+4  .Telerik.Windows.Controls.ChartView.ChartSeriesD   ItemsSourceD �� *ChartDataSource+<  6Telerik.Windows.Controls.ChartView.PointTemplateSeriesE   PointTemplateE �� .R�A� $��6��$��6��
��� d ��� $ 
Horizontal=�.+��� $��Informations��$��0,10,0,0q�$��315.712��$
��Left=�.��� $��0q�.+��� -   $ MyLegend��$
��Wrap=�$��0,4.5,7.667,0q�$	��Top=�$��45.833���� $
��Left=�$
��Wrap=�$	��Top=�$��28.666,55.333,0,0q�$��100.757���� F � Language$F fr-fr�$�Point sélectionné :���� $
��Left=�$
��Wrap=�$��Date: ��$	��Top=�$��46.843,78.383,0,0q��� $
��Left=�$
��Wrap=�$��31.904,99.343,0,0q�$��45.689���� $F fr-fr�$�	Quintile:���� -   $
 Date��$
��Left=�$
��Wrap=�$	��Top=�$��84,78.383,0,0q��� -   $ Valeur��$
��Left=�$
��Wrap=�$	��Top=�$��84,99.343,0,0q��� $
��Left=�$��106.833��$��
330,20,0,0q�$	��Top=�$	��210��. �/� .T�J� .*�.*��� $F fr-fr�$���Par défaut, sont affichées les valeurs ayant une différence de rangs supérieure à 1. Cochez la case pour afficher les valeurs ayant une différence de rangs supéieure ou égale à 1.���� $
��Left=�$
��Wrap=�$n��hChemin exportation PDF: \\mede1\partage\,FGA Front Office\02_Gestion_Actions\00_BASE\02_EVOLUTION SCORES��$��10,0,0,0q�$	��460��$��41��$��Bottom=�