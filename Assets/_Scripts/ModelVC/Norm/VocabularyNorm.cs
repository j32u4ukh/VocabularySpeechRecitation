namespace VTS
{
    // Norm: 資料物件，只需要宣告資料物件持有的變數
    public class VocabularyNorm
    {
        public string vocabulary;
        public string description;

        public VocabularyNorm(string vocabulary, string description)
        {
            this.vocabulary = vocabulary;
            this.description = description;
        }
    }
}
