using System.Windows.Controls;

namespace FrontV2.Action.Reco
{
    interface IView
    {
        void AddTextBlockToGrid(TextBlock b);
        void AddRichTextBoxToGrid(RichTextBox b);
    }
}
