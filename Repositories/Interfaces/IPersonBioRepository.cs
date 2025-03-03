using Convenience.org.Components.Widgets.Cards;
using Convenience.org.Models;
using System;
using System.Collections.Generic;

namespace Convenience.org.Repositories.Interfaces;

public interface IPersonBioRepository
{
    PersonBioItem GetPersonBioRepository(List<Guid> webPageGuids);
    CompanyAndFeatureProfileCardItem GetCompanyandFeatureProfileRepository(List<Guid> webPageGuids);
}
