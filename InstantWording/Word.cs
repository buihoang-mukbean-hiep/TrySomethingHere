namespace InstantWording
{
    public class Word
    {
        public int Id {  get; set; }
        public string? Kanji { get; set; }
        public string? Hiragana { get; set; }
        public string? Kanji_Vietnamese { get; set; }
        public string? Definition { get; set; }
        public int RememberLevel { get; set; }
        public DateTime LastReviewDate { get; set; }
        public int NextReviewLeft { get; set; }

        private int _reviewTimes;
        public int ReviewTimes { 
            get { return _reviewTimes; } 
            set { _reviewTimes = value < 0 ? 0 : value; } }
    }
}
