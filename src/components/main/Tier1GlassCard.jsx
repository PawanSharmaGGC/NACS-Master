import React from "react";
import Tier1GlassStyle from "../../stylesheets/Tier1GlasStyle.module.css";
import Tier1Cover from "../../assests/images/tier1hero.png";
import { Card } from "react-bootstrap";
import { SideRoundButton } from "../ui-components/SideRoundButton";
import { BackToLog } from "../ui-components/BackToLog";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";
import { DateTimeArticleSummary } from "../ui-components/DateTimeArticleSummary";

export const Tier1GlassCard = () => {
  return (
    <div>
      <Card className={`border-0 ${Tier1GlassStyle.main_card}`}>
        <Card
          className={`border-0 p-0 bg-trans ${Tier1GlassStyle.section_card}`}
        >
          <Card.Body
            className={`p-4 flex-fill ${Tier1GlassStyle.section_body_card}`}
          >
            <div className="position-relative float-lg-start">
              <div className={` ${Tier1GlassStyle.image_container}`}>
                <div className={` ${Tier1GlassStyle.blur_overlay}`}></div>
                <img
                  className={` ${Tier1GlassStyle.hero_card}`}
                  src={Tier1Cover}
                  width="341px"
                  alt=""
                />
              </div>
              <div
                className={`position-absolute text-light ${Tier1GlassStyle.txt_over_img}`}
              >
                <EyebrowTitle
                  title="THRIVR"
                  leftBorderColor="border_left_white"
                  titleColor="color-FFFFFF"
                />
                <div className=" text-start fw-semibold fs-4 mb-3">
                  <span>Retailers are Disrupting the Balance of power</span>
                </div>
                <div className="mb-1">
                  <DateTimeArticleSummary
                    textColor="color-FFFFFF"
                    date="24 March 2024"
                    article="10 min read"
                    divider={true}
                    dividerColor="color-62BD19"
                    fontSize="fs-5"
                  />
                </div>
                <div className="float-start mt-3">
                  <SideRoundButton name="Read Articale" />
                </div>
              </div>
            </div>
          </Card.Body>
        </Card>
        <div className="mt-3 mb-3">
          <BackToLog />
        </div>
      </Card>
    </div>
  );
};
