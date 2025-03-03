using System.Collections.Generic;
using System.Linq;
using Convenience.org.Components.Widgets.CAperatureHero;
using Convenience.org.Components.Widgets.Heros.SponsorHero;
using Convenience.org.Components.Widgets.Heros.Tier1GlassSuperHeroCard;
using Convenience.org.Components.Widgets.Heros.Tier1SuperHero;
using Convenience.org.Components.Widgets.ProductHero;
using Convenience.org.Components.Widgets.TextOnlyHero;
using Convenience.org.Components.Widgets.Tier2Hero;
using Convenience.org.Components.Widgets.Tier3Hero;

namespace Convenience.org
{
    public class WidgetAreaIdentifier
    {
        public static IEnumerable<string> Hero_Widgets { get; private set; } = [
            Tier2HeroWidget.IDENTIFIER,
            CAperatureHeroWidget.IDENTIFIER,
            ProductHeroWidget.IDENTIFIER,
            SponsorHeroWidget.IDENTIFIER,
            TextOnlyHeroWidget.IDENTIFIER,
            Tier1SuperHeroWidget.IDENTIFIER,
            Tier1GlassSuperHeroCardWidget.IDENTIFIER,
            Tier3HeroWidget.IDENTIFIER,
        ];

    }
}
