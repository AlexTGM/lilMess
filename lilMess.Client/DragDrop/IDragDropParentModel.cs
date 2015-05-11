namespace lilMess.Client.DragDrop
{
    using System.Collections.ObjectModel;

    public interface IDragDropParentModel : IDragDropChildenModel
    {
        ObservableCollection<IDragDropChildenModel> Children { get; }

        bool CanAcceptChildren { get; }          
    }
}