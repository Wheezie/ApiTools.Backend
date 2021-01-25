using ApiTools.Domain.Enum;

namespace ApiTools.Domain.Data.Base
{
    public class Post : ContentBase<uint>
    {
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Visibility state
        /// </summary>
        public Visibility Visibility { get; set; }
    }
}
