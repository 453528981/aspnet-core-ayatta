namespace Ayatta.Model
{
    public interface IViewModel
    {

    }
    public abstract class ViewModel : IViewModel
    {
        private const string TitleSuffix = " - Tiantian";

        private string title;

        public string Title { get { return title + TitleSuffix; } set { title = value; } }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public string SelectedMenu { get; set; }

    }

    public class ViewModel<T> : ViewModel
    {
        public T Data { get; set; }

        public ViewModel():this(default(T))
        {

        }
        public ViewModel(T data)
        {
            Data = data;
        }
    }
    
    
    public class ViewModel<T, TExtra> : ViewModel<T>
    {
        public TExtra Extra { get; set; }       
        
        
        public ViewModel(T data,TExtra extra):base(data)
        {
            
        }
    }   
    
}
