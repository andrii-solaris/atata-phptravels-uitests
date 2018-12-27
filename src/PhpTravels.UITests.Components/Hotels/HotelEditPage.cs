using Atata;
using System;

namespace PhpTravels.UITests.Components
{
    using _ = HotelEditPage;

    public class HotelEditPage : Page<_>
    {
        [FindByName(TermCase.LowerMerged)]
        [RandomizeStringSettings("AT Hotel {0}")]
        public TextInput<_> HotelName { get; private set; }

        public RichTextEditor<_> HotelDescription { get; private set; }        

        [FindById("s2id_searching")]
        public AutoCompleteSelect<_> Location { get; private set; }

        public ButtonDelegate<HotelsPage, _> Submit { get; private set; }

        [FindByName("hotelstars")]        
        public Select<int, _> Hotelstars { get; private set; }

        [FindByName("hoteltype")]
        public Select<string, _> Hoteltype { get; private set; }

        [FindByName("ffrom")]
        public TextInput<_> FromField { get; private set;}

        [FindByName("fto")]
        public TextInput<_> ToField { get; private set; }

        public static DateTime RandomDate()
        {
            Random gen = new Random();            
            return DateTime.Today.AddDays(gen.Next(5 * 365));
        }
    }
}
