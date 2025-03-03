import React from "react";
import { Card } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";
import Tier3HeroStyle from "../../stylesheets/Tier3HeroStyle.module.css";
import Tier3 from "../../assests/images/tier-3.png";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";
import { DateTimeArticleSummary } from "../ui-components/DateTimeArticleSummary";

export const Tier3Hero = () => {
  return (
    <div>
      <Card className={`p-3 ${Tier3HeroStyle.main_card}`}>
        <div>
          <EyebrowTitle
            title="BEST OF THE WEEK"
            leftBorderColor="border_left_green"
          />
          <div className="">
            <div className="position-relative">
              <img
                src={Tier3}
                alt="Highway Interchange"
                className={`img-fluid ${Tier3HeroStyle.img_highway_interchange}`}
              />
              <div
                className={`text-start position-absolute fw-bolder ${Tier3HeroStyle.overlay_txt_img_pos}`}
              >
                <span className={`${Tier3HeroStyle.overlay_txt_img}`}>
                  60 Years Of Selling Gasoline Episode 444
                </span>
              </div>
            </div>
          </div>
          <div className="pt-5">
            <DateTimeArticleSummary
              textColor="color-262d61"
              date="24 March 2024"
              article="5min Read"
              divider={true}
              dividerColor="color-62BD19"
              fontSize="fs-5"
            />
          </div>
        </div>
        <div className="mt-5">
          <BackToLog />
        </div>
      </Card>
    </div>
  );
};
