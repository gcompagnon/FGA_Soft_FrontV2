   M S B A M L   `   `   `  ����  FrontV25    /FrontV2.Action.ScoreChange.View.ScoreChangeView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   l hTelerik.Windows.Documents.Core, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7x tTelerik.Windows.Controls.DataVisualization, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7g cTelerik.Windows.Documents, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.RibbonView, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7c	 _Telerik.Windows.Maths, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o
 kTelerik.Windows.Controls.GridView, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7| xTelerik.Windows.Documents.FormatProviders.Xaml, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7| xTelerik.Windows.Documents.FormatProviders.Html, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r nTelerik.Windows.Controls.ImageEditor, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7 {Telerik.Windows.Documents.FormatProviders.OpenXml, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7{ wTelerik.Windows.Documents.FormatProviders.Rtf, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Input, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7f bTelerik.Windows.Controls, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7t pTelerik.Windows.Controls.RichTextBoxUI, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Chart, Version=2014.3.1021.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7_telerik1http://schemas.telerik.com/2008/xaml/presentation     	 
            5   	   Zd2http://schemas.microsoft.com/expression/blend/2008     	 
            6K   dmc;http://schemas.openxmlformats.org/markup-compatibility/2006     	 
            6�   -   5          �Name$  window��5   �    � Icon$ /fga.ico��5   	    � Title$ Changements de scores��5   	   $
��1017��6'   $	��731��64    � WindowState$ 	Maximized=�6A    � 	Resources 5       )  %GridViewGroupPanelStyleExpandCollapse �� 
TargetType: 
 4Telerik.Windows.Controls.GridView.GridViewGroupPanel �� Property �� Value �� 
TargetType	 � UseLayoutRounding'  !System.Windows.VisualStateManager
   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard �� TargetProperty �� 
TargetName R� KeyTime R� Value  System.Windows.Visibility �� Orientation ��Name J� IsHitTestVisible2  ,Telerik.Windows.Controls.LocalizationManager   ResourceKey K� IsItemsHost �� EndPoint �� 
StartPoint � Color � Offset��%  &	        �� 5	   
   "  6>   .�5
      ��  � �� 5      �� 5      "  6&   .R�5      � 	 . 
 5        5   "   Grouping��. 5   &     NoGroups��62     5   &   	HasGroups��. 5   *   �� .��5   .   T� $ 
Visibility�6L   $ 	panelText��6s   ..�5   2   i� $ 0��6I    5   6    5   :   	Collapsed=�6P   5   3   5   /   T� 5   .   $ 
Visibility�6L   $ panelTextGrouped��6s   ..�5   2   i� $ 0��6I    5   6    5   :   Visible=�6N   5   3   5    /   5!   +   5"   '   5#   #   .+�5%      �� #��z��6%   #��z��6Q   ._�5&   "   �� ���	#FFEFF6FF$��1q�6A   #��z��6U   5'      �� 5(      $��0,0,8,0q�6)   $ 
Horizontal=�6:   .+�5)   "   �� -   5        $��2q�5)   (   $��
expand.png��63   6G   �� 5*   "   -   5        $��2q�5*   (   $��collapse.png��63   6I   �� 5+   "   $ 	panelText��6,   $	��9.5(�6?   #��z��6N    .  #��z��6�   $ GridViewGroupPanelText��6�   $b��\Faire glisser l'en-tête de la colonne et le déposer près de la colonne pour les regrouper��6  #��z��6o  �� 6�  $b�\Faire glisser l'en-tête de la colonne et le déposer près de la colonne pour les regrouper��6�  6  �� 5,   "   $ panelTextGrouped��6,   #��z��6F    .  #��z��6�   $&  GridViewGroupPanelTopTextGrouped��6�   $��Regroupé par��6  $y�	Collapsed=�6$  #��z��6;  �� 6{  $�Regroupé par��6  6�  �� 5-   "   $��Center=�61   5.      5/      50      52      �� 53       � �� 54      �� 55      .R�56      ��  . $ 
Horizontal=�68   $��Stretch=�6Q   57      59      �� 5:       � ��$ 32��6*   �� 5;       � ��$ 0q�6'   �� 5<       � ��$ 10,0,3,0q�6(   �� 5=       � �� 5>      �� 5?      $ 0.5,1&�6*   $ 0.5,0&�6;   .��5@      � $ 	#FFDFDFDF��6'   $ 1Q�69   � 5A      $ 	#FFBABABA��6'   5B      5D      �� 5E       � �� �	#FF8484846,   �� 5F       � ��$ 1q�60   �� 5G       � ��$ Center=�69   �� 5H       � ��$
 Left=�6;   �� 5I       �@J�SnapsToDevicePixels . 64   5J      5K      .��5L      � .+�5M   
   �� $��Filtres��6   $
��Left=�6$   $	��Top=�6?   $��	10,10,0,0q�6W   $	��315��6j   $	��145��6v   .��5N      � $��0q�6   .+�5O      �� $��	30,19,0,0q�6   $	��Top=�60   $
��Left=�6H   $ 
Horizontal=�6c   .+�5P      �� $
��Wrap=�6    $��Center=�64   $��22��6Q   $��Date 1 (Récente)��6]   $��95��6u   �� 5Q      $	��5,0q�5R   Y   $��Center=�6f   ��6   �� *Dates+ �� SelectedItem 6%   �� *SelectedDate1+ �� Mode$ TwoWay=�6f   5S      �� 5T      $��	30,46,0,0q�6   $
��Left=�60   $	��Top=�6K   $ 
Horizontal=�6c   .+�5U      �� $
��Wrap=�6    $��Date 2 (Ancienne)��64   $��Center=�6M   $��22��6j   $��95��6v   �� 5V      $��Center=�6   $	��5,0q�5W   Y   ��6   �� *Dates+ 6%   �� *SelectedDate2+$ TwoWay=�6Y   5X      (  "Telerik.Windows.Controls.RadButton  5Y      -   5        $��	 Charger ��5Y   $   $��205.41,86.08,0,0q�68   $
��Left=�5Z      $	��Top=�5[      $��25.96��6   $��58.59��6-   �� 5\      -   5        $��Filter1��5\      $$��Écarts rangs inférieurs à 1��6*   $��319,108.167,-231.15,-6q�6P   �� 5]      -   5        $��Exporter PDF��5]      $��99.712,87.04,114,10q�60    J� RenderTransformOrigin$ -0.154,3.557&�6M   $��Center=�6�   $��Center=�6�   $��25��6�   $��89.288��6�   5^      5_      * 
 $Telerik.Windows.Controls.RadGridView  5`   
   -   5          Name$ ChangesGrid��5`      ;	 
 5Telerik.Windows.Controls.GridView.GridViewDataControl 	  ClipboardCopyMode$	 All=�5b       	  CanUserDeleteRows .   	  CanUserInsertRows  .  ! 	  
IsReadOnly! . " 	  AlternateRowBackground
" �����8
 
 2Telerik.Windows.Controls.GridView.BaseItemsControl# 
  AlternationCount$# 2��5g      $ 	  SelectionMode$$ Extended=�5h      % 	  ClipboardPasteMode$% AllSelectedCells=�5i      & 	  AutoGenerateColumns& . ' 	  SelectionUnit$' FullRow=�5l      *  $Telerik.Windows.Controls.DataControl$(   IsSynchronizedWithCurrentItem$
( TrueY�5m      $��555,10,10,10q�5n      !) 	  EnableColumnVirtualization) .  * 	  EnableRowVirtualization* .  + 	  GroupPanelStyle#+ �   5q      ,   ItemsSource, 5a      �� *ChangesDataSource+- 	  SortDescriptors- 5s      )  #Telerik.Windows.Data.SortDescriptor  5t      .   Member$. SECTOR��6)   -  'Telerik.Windows.Data.SortDescriptorBase/   SortDirection$/ 	Ascending=�5u        5v      $. INDUSTRY��6)   $/ 	Ascending=�5w      -  'Telerik.Windows.Controls.RadContextMenu0   ContextMenu0 5z        5{      -   5        .@�5}      *  $Telerik.Windows.Controls.RadMenuItem  -	   5        $+��%Montrer l'évolution sur le graphique��5}   +   1   IconTemplate1 5      �� 5�      .R�5�   "   �� $)��#/FrontV2;component/Images/graph.png��6(   $��UniformToFill=�6U   5�      5�        5�      $��Fiches Valeurs��6*   .@�5�        -
   5        $��Ouvrir fiche excel��5�   .   1 5�      �� 5�   "   .R�5�   &   �� $)��#/FrontV2;component/Images/excel.png��6,   $��UniformToFill=�6Y   5�   #   5�        5�      -   5        $��Ouvrir fiche pdf��5�   .   1 5�      �� 5�   "   .R�5�   &   �� $'��!/FrontV2;component/Images/pdf.png��6,   $��UniformToFill=�6W   5�   #   5�      5�        5�      $��Liens Bloomberg��6*   .@�5�        -   5        $	��DES��5�   .   5�        5�      -   5        $��BQ��5�   .   5�        5�      -   5        $
��HCPI��5�   .   5�        5�      -   5        $	��WGT��5�   .   5�        5�      -   5        $��GR��5�   .   5�      3  -Telerik.Windows.Controls.RadMenuSeparatorItem  5�        5�      -   5        $	��BRC��5�   .   5�        5�      -   5        $	��EVT��5�   .   5�        5�      -   5        $	��ICN��5�   .   5�        5�      -   5        $	��EEG��5�   .   5�        5�        5�      -   5        $
��EQRV��5�   .   5�        5�      -   5        $
��PBAR��5�   .   5�        5�      -   5        $	��ANR��5�   .   5�        5�        5�      -   5        $��GIP 5��5�   .   5�        5�      -   5        $��GIP 30��5�   .   5�      5�      5�      5�      �� 5�   
   $��10,160,0,10q�6   $	��540��6*   $
��Left=�66   .+�5�      0  *Telerik.Windows.Controls.RadCartesianChart  -   5        2  Name$2 quintilesChart��5�   (   $
��10,0q�6@   $	��300��6N   0 5�        5�      .@�5�        -   5        $��Copier dans Paint��5�   .   5�      5�      5  /Telerik.Windows.Controls.ChartView.RadChartBase3   	Behaviors3 5�      ?  9Telerik.Windows.Controls.ChartView.ChartSelectionBehavior  5�      -   5        4   DataPointSelectionMode$4 Single=�5�   5   6U   @  :Telerik.Windows.Controls.ChartView.ChartPanAndZoomBehavior  5�      5   ZoomMode$5 
Horizontal=�66   6   PanMode$6 
Horizontal=�6L   7   HorizontalAxis7 5�      8  2Telerik.Windows.Controls.ChartView.CategoricalAxis  5�      -  'Telerik.Windows.Controls.ChartView.Axis8   LabelFitMode$8 Rotate=�6.   9   LabelRotationAngle$9 90Q�5�   .   :   LabelInterval: 6F   �� *
Interval+;   VerticalAxis; 5�      3  -Telerik.Windows.Controls.ChartView.LinearAxis  5�      6  0Telerik.Windows.Controls.ChartView.NumericalAxis<   MaximumA  ;Telerik.Windows.Controls.Converters.StringToDoubleConverter$< 6 6)   =   Minimum$= 0 65   1  +Telerik.Windows.Controls.ChartView.LineAxis>   	IsInverse> . ?   	MajorStep$? 1 6R   @   Series.@ 5�      3  -Telerik.Windows.Controls.ChartView.LineSeries  A  ;Telerik.Windows.Controls.ChartView.CategoricalStrokedSeriesA   Stroke
A � � �:  4Telerik.Windows.Controls.ChartView.CategoricalSeriesB   CategoryBindingB 5�      �� *CategoriesSelectorQuint+C   ValueBindingC 5�      �� *ValuesSelectorQuint+4  .Telerik.Windows.Controls.ChartView.ChartSeriesD   ItemsSourceD 5�      �� *ChartDataSource+<  6Telerik.Windows.Controls.ChartView.PointTemplateSeriesE   PointTemplateE 5�      �� 5�      .R�5�      A� $��6��6&   $��6��61   
��� d �6;   5�      5�      5�      �� 5�      $ 
Horizontal=�6   .+�5      �� $��Informations��6   $��0,10,0,0q�61   $��315.712��6C   $
��Left=�6S   .��5     � $��0q�6   .+�5     �� -   5        $ MyLegend��5  $   $
��Wrap=�66   $��0,4.5,7.667,0q�6J   $	��Top=�6a   $��45.833��6y   �� 5     $
��Left=�6$   $
��Wrap=�6?   $	��Top=�6S   $��28.666,55.333,0,0q�6k   $��100.757��6�   �� 6�   F � Language$F fr-fr�6�   $�Point sélectionné :��6�   6�   �� 5     $
��Left=�6$   $
��Wrap=�6?   $��Date: ��6S   $	��Top=�6a   $��46.843,78.383,0,0q�6y   �� 5     $
��Left=�6$   $
��Wrap=�6?   $��31.904,99.343,0,0q�6S   $��45.689��6n   �� 6~   $F fr-fr�6�   $�	Quintile:��6�   6�   �� 5     -   5        $
 Date��5  $   $
��Left=�62   $
��Wrap=�6M   $	��Top=�6a   $��84,78.383,0,0q�6y   �� 5     -   5        $ Valeur��5  $   $
��Left=�64   $
��Wrap=�6O   $	��Top=�6c   $��84,99.343,0,0q�6{   5     5	     5
     5     �� 5  
   $
��Left=�6   $��106.833��61   $��
330,20,0,0q�6B   $	��Top=�6V   $	��210��6n   . �5     /� .T�5     J� .*�6   .*�5     �� $F fr-fr�6   $���Par défaut, sont affichées les valeurs ayant une différence de rangs supérieure à 1. Cochez la case pour afficher les valeurs ayant une différence de rangs supéieure ou égale à 1.��6+   5     5     5     �� 5  
   $
��Left=�6   $
��Wrap=�6/   $n��hChemin exportation PDF: \\mede1\partage\,FGA Front Office\02_Gestion_Actions\00_BASE\02_EVOLUTION SCORES��6C   $��10,0,0,0q�6�   $	��460��6�   $��41��6�   $��Bottom=�6�   5     5     