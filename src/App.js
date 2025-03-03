import "./App.css";
import "./stylesheets/common.css";
import { BioCard } from "./components/main/BioCard";
import { EventSpeakerCard } from "./components/main/EventSpeakerCard";
import { Tier2Hero } from "./components/main/Tier2Hero";
import { InlineRelatedPost } from "./components/main/InlineRelatedPost";
import { RelatedContentCard } from "./components/main/RelatedContentCard";
import { VideoPlayerComponent } from "./components/main/VideoPlayerComponent";
import { TestimonialCarousel } from "./components/main/TestimonialCarousel";
import { TestimonialStack } from "./components/main/TestimonialStack";
import { TextOnlyHero } from "./components/main/TextOnlyHero";
import { CopyBlock } from "./components/main/CopyBlock";
import { Tier1Hero } from "./components/main/Tier1Hero";
import { PullQuote } from "./components/main/PullQuote";
import { SingleTestimonial } from "./components/main/SingleTestimonial";
import { SubscribeComponent } from "./components/main/SubscribeComponent";
import { DeepDive } from "./components/main/DeepDive";
import { ProductHero } from "./components/main/ProductHero";
import { Footer } from "./components/main/Footer";
import { SponsorHero } from "./components/main/SponsorHero";
import { Route, Routes } from "react-router-dom";
import { DownloadReport } from "./components/main/DownloadReport";
import { SignInForm } from "./components/main/SignInForm";
import { FormTrans } from "./components/main/FormTrans";
import { NavbarComponent } from "./components/ui-components/Navbar";
import { Statistics } from "./components/main/Statistics";
import { Tier1ContentCardFeatured } from "./components/main/Tier1ContentCardFeatured";
import { Tier1ContentCard } from "./components/main/Tier1ContentCard";
import { Tier1GlassCard } from "./components/main/Tier1GlassCard";
import { ExternalSiteCard } from "./components/main/ExternalSiteCard";
import { RecommendedCardCarousel } from "./components/main/RecommendedCardCarousel";
import { CTACardNoImage } from "./components/main/CTACardNoImage";
import { FAQCard } from "./components/main/FAQCard";
import { PriceCard } from "./components/main/PriceCard";
import { FeaturedProfileCard } from "./components/main/FeaturedProfileCard";
import { FeaturedContentCard } from "./components/main/FeaturedContentCard";
import { CompanyCard } from "./components/main/CompanyCard";
import { Tier2ContentCard } from "./components/main/Tier2ContentCard";
import { Tier3ContentCard } from "./components/main/Tier3ContentCard";
import { Tier4ContentCard } from "./components/main/Tier4ContentCard";
import { EventCard } from "./components/main/EventCard";
import { ExpandableContentCard } from "./components/main/ExpandableContentCard";
import { EventDetails } from "./components/main/EventDetails";
import { CApertureHero } from "./components/main/CApertureHero";
import { Tier3Hero } from "./components/main/Tier3Hero";
import { EventVideoCard } from "./components/main/EventVideoCard";
import { EventHero } from "./components/main/EventHero";
import { EventCarousel } from "./components/main/EventCarousel";
import { ContentFeedFilter } from "./components/ui-components/ContentFeedFilter";
import { EventSearchSortFilter } from "./components/ui-components/EventSearchSortFilter";
import { SearchComponent } from "./components/ui-components/SearchComponent";
import { MembershipCard } from "./components/main/MembershipCard";
import { Sponsor } from "./components/main/Sponsor";

const routeData = [
  {
    id: 1,
    title: "Bio Card",
    url: "/bio-card",
  },
  {
    id: 2,
    title: "Event Speaker Card",
    url: "/EventSpeakerCard",
  },
  {
    id: 3,
    title: "Tier2 Hero",
    url: "/Tier2Hero",
  },
  {
    id: 4,
    title: "Related Content Card",
    url: "/RelatedContentCard",
  },
  {
    id: 5,
    title: "Video Player Component",
    url: "/VideoPlayerComponent",
  },
  {
    id: 6,
    title: "Testimonial Stack",
    url: "/TestimonialStack",
  },
  {
    id: 7,
    title: "Inline Related Post",
    url: "/InlineRelatedPost",
  },
  {
    id: 8,
    title: "Testimonial Carousel",
    url: "/TestimonialCarousel",
  },
  {
    id: 9,
    title: "Text-only-hero",
    url: "/TextOnlyHero",
  },
  {
    id: 10,
    title: "Copy Block",
    url: "/CopyBlock",
  },
  {
    id: 11,
    title: "Pull Quote",
    url: "/PullQuote",
  },
  {
    id: 12,
    title: "Single Testimonial",
    url: "/SingleTestimonial",
  },
  {
    id: 13,
    title: "Subscribe Component",
    url: "/SubscribeComponent",
  },
  {
    id: 14,
    title: "Tier-1-hero",
    url: "/Tier1Hero",
  },
  {
    id: 15,
    title: "Deep Dive",
    url: "/DeepDive",
  },
  {
    id: 16,
    title: "Product Hero",
    url: "/ProductHero",
  },
  {
    id: 17,
    title: "Footer Robust",
    url: "/Footer",
  },
  {
    id: 18,
    title: "Sponsor Hero",
    url: "/SponsorHero",
  },
  {
    id: 19,
    title: "Download Form",
    url: "/DownloadForm",
  },
  {
    id: 20,
    title: "Sign Up",
    url: "/SignUp",
  },
  {
    id: 21,
    title: "Transparent Form",
    url: "/FormTrans",
  },
  {
    id: 22,
    title: "Statistics",
    url: "/Statistics",
  },
  {
    id: 23,
    title: "Tier-1 Content Card Fetured",
    url: "/Tier1ContentCardFeatured",
  },
  {
    id: 24,
    title: "Tier-1 Glass Card",
    url: "/Tier1GlassCard",
  },
  {
    id: 25,
    title: "Tier-1 Content Card",
    url: "/Tier1ContentCard",
  },
  {
    id: 26,
    title: "External Site Card",
    url: "/ExternalSiteCard",
  },
  {
    id: 27,
    title: "Recommended Card Carousel",
    url: "/RecommendedCardCarousel",
  },
  {
    id: 28,
    title: "CTA Card No Image",
    url: "/CtaCardNoImage",
  },
  {
    id: 29,
    title: "FAQ Card",
    url: "/FAQCard",
  },
  {
    id: 30,
    title: "Price Card",
    url: "/PriceCard",
  },
  {
    id: 31,
    title: "Featured Profile Card",
    url: "/profile",
  },
  {
    id: 32,
    title: "Featured Content Card",
    url: "/featured-content-card",
  },
  {
    id: 33,
    title: "Company Card",
    url: "/company-card",
  },
  {
    id: 34,
    title: "Tier 2 Content Card",
    url: "/tier2-content-card",
  },
  {
    id: 35,
    title: "Tier 3 Content Card",
    url: "/tier3-content-card",
  },
  {
    id: 36,
    title: "Tier 4 Content Card",
    url: "/tier4-content-card",
  },
  {
    id: 37,
    title: "Event Card",
    url: "/event-card",
  },
  {
    id: 38,
    title: "Expandable Content Card",
    url: "/expandable-content-card",
  },
  {
    id: 39,
    title: "Event Details",
    url: "/event-details",
  },
  {
    id: 40,
    title: "C Aperture Hero",
    url: "/c-aperture-hero",
  },
  {
    id: 41,
    title: "Tier 3 Hero",
    url: "/tier-3-hero",
  },
  {
    id: 42,
    title: "Event Video Card",
    url: "/event-video-card",
  },
  {
    id: 43,
    title: "Event Hero Card",
    url: "/event-hero-card",
  },
  {
    id: 44,
    title: "Event Carousel Card",
    url: "/event-carousel-card",
  },
  {
    id: 45,
    title: "Membership Card",
    url: "/membership-card",
  },
  {
    id: 46,
    title: "Sponsor",
    url: "/sponsors",
  },
];

function App() {
  return (
    <div className="App">
      <NavbarComponent tempProps={routeData} />

      <ContentFeedFilter />

      <div className="mt-3">
        <Routes>
          <Route
            path="/bio-card"
            element={
              <>
                <p className="bg-warning text-dark mb-3">25th Sep</p>

                <span className="bg-info text-dark">Bio Card</span>
                <BioCard />
              </>
            }
          />
          <Route
            path="/EventSpeakerCard"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">26th Sep</p>

                <span className="bg-info text-dark">Event Speaker Card</span>
                <EventSpeakerCard />
              </>
            }
          />
          <Route
            path="/Tier2Hero"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">26th Sep</p>

                <span className="bg-info text-dark">Tier2 Hero</span>
                <Tier2Hero />
              </>
            }
          />
          <Route
            path="/RelatedContentCard"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">26th Sep</p>

                <span className="bg-info text-dark">Related Content Card</span>
                <RelatedContentCard />
              </>
            }
          />
          <Route
            path="/VideoPlayerComponent"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">26th Sep</p>

                <span className="bg-info text-dark">
                  Video Player Component
                </span>
                <VideoPlayerComponent />
              </>
            }
          />
          <Route
            path="/TestimonialStack"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">27th Sep</p>

                <span className="bg-info text-dark">Testimonal Stack</span>
                <TestimonialStack />
              </>
            }
          />
          <Route
            path="/InlineRelatedPost"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">27th Sep</p>

                <span className="bg-info text-dark">Inline Related Post</span>
                <InlineRelatedPost />
              </>
            }
          />
          <Route
            path="/TestimonialCarousel"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">30th Sep</p>

                <span className="bg-info text-dark">Testimonal Carousel</span>
                <TestimonialCarousel />
              </>
            }
          />
          <Route
            path="/TextOnlyHero"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">30th Sep</p>

                <span className="bg-info text-dark">Text-only-hero</span>
                <TextOnlyHero />
              </>
            }
          />
          <Route
            path="/CopyBlock"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">30th Sep</p>

                <span className="bg-info text-dark">Copy Block</span>
                <CopyBlock />
              </>
            }
          />
          <Route
            path="/PullQuote"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">30th Sep</p>

                <span className="bg-info text-dark">Pull Quote</span>
                <PullQuote />
              </>
            }
          />
          <Route
            path="/SingleTestimonial"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">30th Sep</p>

                <span className="bg-info text-dark">Single Testimonial</span>
                <SingleTestimonial />
              </>
            }
          />
          <Route
            path="/SubscribeComponent"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">30th Sep</p>

                <span className="bg-info text-dark">Subscribe Component</span>
                <SubscribeComponent />
              </>
            }
          />
          <Route
            path="/Tier1Hero"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">1st Oct</p>

                <span className="bg-info text-dark">Tier-1-hero</span>
                <Tier1Hero />
              </>
            }
          />
          <Route
            path="/DeepDive"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">1st Oct</p>

                <span className="bg-info text-dark">Deep Dive</span>
                <DeepDive />
              </>
            }
          />
          <Route
            path="/ProductHero"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">1st Oct</p>

                <span className="bg-info text-dark">Product Hero</span>
                <ProductHero />
              </>
            }
          />
          <Route
            path="/Footer"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">3rd Oct</p>

                <span className="bg-info text-dark">Footer Robust</span>
                <Footer />
              </>
            }
          />
          <Route
            path="/SponsorHero"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">3rd Oct</p>

                <span className="bg-info text-dark">Sponsor Hero</span>
                <SponsorHero />
              </>
            }
          />

          <Route
            path="/DownloadForm"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">16th Oct</p>

                <span className="bg-info text-dark">Download Form</span>
                <DownloadReport />
              </>
            }
          />

          <Route
            path="/SignUp"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">16th Oct</p>

                <span className="bg-info text-dark">Sign Up</span>
                <SignInForm />
              </>
            }
          />
          <Route
            path="/FormTrans"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">16th Oct</p>

                <span className="bg-info text-dark">Transparent Form</span>
                <FormTrans />
              </>
            }
          />
          <Route
            path="/Statistics"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">23th Oct</p>

                <span className="bg-info text-dark">Statistics</span>
                <Statistics />
              </>
            }
          />
          <Route
            path="/Tier1ContentCardFeatured"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">23th Oct</p>

                <span className="bg-info text-dark">
                  Tier-1 Content Card Featured
                </span>
                <Tier1ContentCardFeatured />
              </>
            }
          />
          <Route
            path="/Tier1ContentCard"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">24th Oct</p>

                <span className="bg-info text-dark">Tier-1 Content Card</span>
                <Tier1ContentCard />
              </>
            }
          />
          <Route
            path="/Tier1GlassCard"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">25th Oct</p>

                <span className="bg-info text-dark">Tier-1 Glass Card</span>
                <Tier1GlassCard />
              </>
            }
          />
          <Route
            path="/ExternalSiteCard"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">25th Oct</p>

                <span className="bg-info text-dark">External Site Card</span>
                <ExternalSiteCard />
              </>
            }
          />
          <Route
            path="/RecommendedCardCarousel"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">28th Oct</p>

                <span className="bg-info text-dark">
                  Recommended Card Carousel
                </span>
                <RecommendedCardCarousel />
              </>
            }
          />
          <Route
            path="/CtaCardNoImage"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">29th Oct</p>

                <span className="bg-info text-dark">CTA Card No Image</span>
                <CTACardNoImage />
              </>
            }
          />
          <Route
            path="/FAQCard"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">29th Oct</p>

                <span className="bg-info text-dark">FAQ Card</span>
                <FAQCard />
              </>
            }
          />
          <Route
            path="/PriceCard"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">29th Oct</p>

                <span className="bg-info text-dark">Price Card</span>
                <PriceCard />
              </>
            }
          />
          <Route
            path="/profile"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">30th Oct</p>

                <span className="bg-info text-dark">Featured Profile Card</span>
                <FeaturedProfileCard />
              </>
            }
          />
          <Route
            path="/featured-content-card"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">30th Oct</p>

                <span className="bg-info text-dark">Featured Content Card</span>
                <FeaturedContentCard />
              </>
            }
          />
          <Route
            path="/company-card"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">30th Oct</p>

                <span className="bg-info text-dark">Company Card</span>
                <CompanyCard />
              </>
            }
          />
          <Route
            path="/tier2-content-card"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">1st Nov</p>

                <span className="bg-info text-dark">Tier2 Content Card</span>
                <Tier2ContentCard />
              </>
            }
          />
          <Route
            path="/tier3-content-card"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">4th Nov</p>

                <span className="bg-info text-dark">Tier3 Content Card</span>
                <Tier3ContentCard />
              </>
            }
          />
          <Route
            path="/tier4-content-card"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">4th Nov</p>
                <span className="bg-info text-dark">Tier4 Content Card</span>
                <Tier4ContentCard />
              </>
            }
          />
          <Route
            path="/event-card"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">6th Nov</p>
                <span className="bg-info text-dark">Event Card</span>
                <EventCard />
              </>
            }
          />
          <Route
            path="/expandable-content-card"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">7th Nov</p>
                <span className="bg-info text-dark">
                  Expandable Content Card
                </span>
                <ExpandableContentCard />
              </>
            }
          />
          <Route
            path="/event-details"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">14th Nov</p>
                <span className="bg-info text-dark mb-3 d-inline-block">
                  Event Search Sort FIlter
                </span>
                <EventSearchSortFilter />
                <p className="bg-warning text-dark mt-3 mb-3">7th Nov</p>
                <span className="bg-info text-dark">Event Details</span>
                <EventDetails />
              </>
            }
          />
          <Route
            path="/c-aperture-hero"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">8th Nov</p>
                <span className="bg-info text-dark">C Aperture Hero</span>
                <CApertureHero />
              </>
            }
          />
          <Route
            path="/tier-3-hero"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">8th Nov</p>
                <span className="bg-info text-dark">Tier 3 Hero</span>
                <Tier3Hero />
              </>
            }
          />
          <Route
            path="/event-video-card"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">11th Nov</p>
                <span className="bg-info text-dark">Event Video Card</span>
                <EventVideoCard />
              </>
            }
          />
          <Route
            path="/event-hero-card"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">18th Nov</p>
                <span className="bg-info text-dark d-inline-block mb-3">
                  Search
                </span>
                <SearchComponent />
                <p className="bg-warning text-dark mt-3 mb-3">12th Nov</p>
                <span className="bg-info text-dark">Event Hero Card</span>
                <EventHero />
              </>
            }
          />
          <Route
            path="/event-carousel-card"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">13th Nov</p>
                <span className="bg-info text-dark">Event Cariusel Card</span>
                <EventCarousel />
              </>
            }
          />
          <Route
            path="/membership-card"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">25th Nov</p>
                <span className="bg-info text-dark">Membership Card</span>
                <MembershipCard />
              </>
            }
          />
          <Route
            path="/sponsors"
            element={
              <>
                <p className="bg-warning text-dark mt-3 mb-3">29th Nov</p>
                <span className="bg-info text-dark">Sponsor</span>
                <Sponsor />
              </>
            }
          />
        </Routes>
      </div>
    </div>
  );
}

export default App;
