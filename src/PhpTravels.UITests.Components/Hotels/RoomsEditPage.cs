using Atata;
using System;

namespace PhpTravels.UITests.Components
{
    using _ = RoomsEditPage;

    public class RoomsEditPage : Page<_>
    {
      [FindByXPath("//span[text()='Room Type']/..")]
      public Link<_> RoomTypeDropdown { get; private set; }

      [FindByIndex(2)]
      [PressEnter]
      public TextInput<_> RoomTypeInput { get; private set; }

      [FindByXPath("//label[text()='Hotel']/..//a")]
      public Link<_> HotelNameDropdown { get; private set; }

      [FindByIndex(4)]
      [PressEnter]
      public TextInput<_> HotelNameInput { get; private set; }

      [FindByName("basicprice")] 
      public NumberInput<_> PriceInput { get; private set; }

      public ButtonDelegate<RoomsPage, _> Submit { get; private set; }
    }        
}
