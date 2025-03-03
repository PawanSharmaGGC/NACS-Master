import React from "react";
import clip from "../../assests/images/Clip path group.png";
import { Card } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";
import CApertureHeroStyle from "../../stylesheets/CApertureHeroStyle.module.css";
import { Breadcrumbs } from "../ui-components/Breadcrumbs";
import ExpandableImage from "../../assests/images/expandable-card-img.jpeg";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";
import { DateTimeArticleSummary } from "../ui-components/DateTimeArticleSummary";

const page = ["Home", "News"];

export const CApertureHero = () => {
  return (
    <div>
      <Card className="p-0 bg-FFFFFF">
        <Card className="border-0">
          <Card.Body className="p-0">
            <div className={`ps-3 d-lg-none d-sm-block d-md-none`}>
              <Breadcrumbs pages={page} />
            </div>
            <div className={`${CApertureHeroStyle.hero_card}`}>
              <div className="flex-grow-1 p-3 p-lg-5 mt-lg-4">
                <EyebrowTitle
                  leftBorderColor="border_left_green"
                  title="Best Of The Week"
                />
                <div className="text-start">
                  <p
                    className={`fw-semibold text-start m-0 mt-2 color-002569 ${CApertureHeroStyle.sponsor_title}`}
                  >
                    Stopping Swipe Fee’s On State Taxes: Illinois’s landmark law
                  </p>
                  <div className={`pt-5 d-lg-none d-sm-block d-md-none`}>
                    <img
                      className="img-fluid brdr-rad-30"
                      src={ExpandableImage}
                      alt="Expandable-Image"
                    />
                  </div>
                  <div className="pt-5">
                    <DateTimeArticleSummary
                      textColor="color-262d61"
                      date="24 March 2024"
                      time="03:00 PM"
                      article="Eastern Time"
                      divider={true}
                      dividerColor="color-868F98"
                      fontSize="fs-5"
                    />
                  </div>
                </div>
              </div>
              <div className={`${CApertureHeroStyle.img_section}`}>
                <img src={clip} width={689} alt="" className="ms-5" />
                <div className={`${CApertureHeroStyle.c_shape_container}`}>
                  <video
                    className={`${CApertureHeroStyle.c_shaped_video}`}
                    autoPlay
                    loop
                    muted
                  >
                    <source
                      src="https://www.shutterstock.com/shutterstock/videos/1096130427/preview/stock-footage-time-lapse-of-modern-city-and-road-aerial-view.webm"
                      type="video/webm"
                    />
                  </video>
                  <div className={`${CApertureHeroStyle.c_shape_cutout}`}></div>
                  <div
                    className={`${CApertureHeroStyle.rect_shape_cutout}`}
                  ></div>
                </div>
              </div>
            </div>
          </Card.Body>
        </Card>
        <div className="mt-4">
          <BackToLog />
        </div>
      </Card>
    </div>
  );
};
