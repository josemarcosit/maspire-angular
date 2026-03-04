namespace angular_vega.Controllers.Resources
{
    public class PhotoResource
    {
        public int Id { get; set; }
        public string FileName { get; set; }
            public string Url
            {
                get { return $"/uploads/{FileName}"; }
            }
    }
}
