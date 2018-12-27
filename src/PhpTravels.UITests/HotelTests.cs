using Atata;
using NUnit.Framework;
using PhpTravels.UITests.Components;
using System;

namespace PhpTravels.UITests
{   

    public class HotelTests : UITestFixture
    {       

        [Test]        
        public void Hotel_Add([Random(1, 5, 1)] int stars)
        {

            var startDate = HotelEditPage.RandomDate();

            LoginAsAdmin();            

            Go.To<HotelsPage>().
                Add.ClickAndGo().
                    HotelName.SetRandom(out string name).
                    HotelDescription.SetRandom(out string description).
                    Hotelstars.Set(stars).
                    Hoteltype.Set("Hotel").
                    Location.Set("London"). 
                    FromField.Set(startDate.ToString("dd/MM/yyyy")).
                    ToField.Set(startDate.AddDays(365).ToString("dd/MM/yyyy")).
                    Submit().
                Hotels.Rows[x => x.Name == name].Should.BeVisible();
        }

        [Test]
        public void Hotel_Edit([Random(1, 5, 1)] int stars)
        {

            var startDate = HotelEditPage.RandomDate();

            LoginAsAdmin();

            Go.To<HotelsPage>().
                Add.ClickAndGo().
                    HotelName.SetRandom(out string name).
                    HotelDescription.SetRandom(out string description).
                    Hotelstars.Set(stars).
                    Hoteltype.Set("Hotel").
                    Location.Set("London").
                    FromField.Set(startDate.ToString("dd/MM/yyyy")).
                    ToField.Set(startDate.AddDays(365).ToString("dd/MM/yyyy")).
                    Submit().
                Hotels.Rows[x => x.Name == name].Should.BeVisible().
                    EditButton.ClickAndGo().
                    Location.Set("Washington").
                    Submit().
                    Hotels.Rows[x => x.Name == name].Content.Should.Contain("Washington");        
        }
    }
}
