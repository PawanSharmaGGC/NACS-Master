import React from "react";
import { Card } from "react-bootstrap";
import Tier1ContentCardFeaturedStyle from "../../stylesheets/Tier1ContentCardFeaturedStyle.module.css";
import tier1ContentImg from "../../assests/images/tier-1-content-featured-img.png";
import { BackToLog } from "../ui-components/BackToLog";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";

export const Tier1ContentCardFeatured = () => {
  return (
    <div>
      <Card className={`p-3 ${Tier1ContentCardFeaturedStyle.main_card}`}>
        <Card className={`${Tier1ContentCardFeaturedStyle.section_card}`}>
          <div className="p-3 ps-4">
            <EyebrowTitle
              title="DESKTOP EYEBROW"
              leftBorderColor="border_left_green"
            />
          </div>
          <div class="row">
            <div class="col-lg-6 col-12">
              <img
                src={tier1ContentImg}
                alt="Tier-1-Content-Image"
                className={`${Tier1ContentCardFeaturedStyle.tier_1_img}`}
              />
            </div>
            <div class="col-lg-6 col-12 text-start p-4">
              <p className="fs-4 color-002569">Working Together</p>
              <p className="fs-4 color-002569">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do
                eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut
                enim ad minim veniam, quis nostrud exercitation ullamco laboris
                nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor
                in reprehenderit in voluptate velit esse cillum dolore eu fugiat
                nulla pariatur. Excepteur sint occaecat cupidatat non proident,
                sunt in culpa qui officia deserunt mollit anim id est laborum.
              </p>
              <p className="m-0">
                <button type="button" className="fs-4 p-0 btn color-0053A5">
                  Learn More
                  <i className="ps-3 pt-2 color-0053A5 fa-regular fa-arrow-right"></i>
                </button>
              </p>
            </div>
          </div>
        </Card>
        <div className="mt-5">
          <BackToLog />
        </div>
      </Card>
    </div>
  );
};
