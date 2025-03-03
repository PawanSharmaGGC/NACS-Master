import React from "react";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";
import SponsorStyle from "../../stylesheets/SponsorStyle.module.css";
import SponsorImage1 from "../../assests/images/sponsor-1.png";
import SponsorImage2 from "../../assests/images/sponsor-2.png";
import SponsorImage3 from "../../assests/images/sponsor-3.png";
import SponsorImage4 from "../../assests/images/sponsor-4.png";
import SponsorImage5 from "../../assests/images/sponsor-5.png";
import SponsorImage6 from "../../assests/images/sponsor-6.png";

const sponsors = [
  { id: 1, sponsor: SponsorImage1 },
  { id: 2, sponsor: SponsorImage2 },
  { id: 3, sponsor: SponsorImage3 },
  { id: 4, sponsor: SponsorImage4 },
  { id: 5, sponsor: SponsorImage5 },
  { id: 6, sponsor: SponsorImage6 },
];

export const Sponsor = () => {
  return (
    <div className="p-3">
      <EyebrowTitle
        leftBorderColor="border_left_green"
        title="Event Sponsor(s)"
      />
      <div className={`mt-lg-5 d-flex  ${SponsorStyle.img_section}`}>
        {sponsors.map((item, index) => (
          <img
            className="img-fluid p-3 p-lg-0"
            key={index}
            src={item.sponsor}
            alt={`sponsor-${index + 1}`}
          />
        ))}
      </div>
    </div>
  );
};
