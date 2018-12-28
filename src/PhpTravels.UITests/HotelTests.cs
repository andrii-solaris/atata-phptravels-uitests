using Atata;
using NUnit.Framework;
using PhpTravels.UITests.Components;
using System;

namespace PhpTravels.UITests
{   

    public class HotelTests : UITestFixture
    {       

        [Test]
        [Order(1)]
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
        [Order(2)]
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
                    Hotels.Rows[x => x.Name == name].Location.Should.Contain("Washington");        
        }

        [Test]
        [Order(3)]
        public void Hotel_Room_Add([Random(1, 5, 1)] int stars)
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

            Go.To<RoomsPage>().
                    Add.ClickAndGo().
                    RoomTypeDropdown.Click().
                    RoomTypeInput.Set("Triple Rooms").
                    HotelNameDropdown.Click().
                    HotelNameInput.Set(name).                    
                    PriceInput.SetRandom(out int price).
                    Submit().
                    Rooms.Rows[x => x.Hotel == name].Price.Should.Contain(price.ToString());                    
        }
    }
}
