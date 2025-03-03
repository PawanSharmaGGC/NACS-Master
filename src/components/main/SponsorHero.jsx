import React from "react";
import { Card } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";
import Wawa from "../../assests/images/wawa.png";
import { SideRoundButton } from "../ui-components/SideRoundButton";
import SponsorHeroStyle from "../../stylesheets/SponsorHeroStyle.module.css";
import clip from "../../assests/images/Clip path group.png";
import { InvertedButton } from "../ui-components/InvertedButton";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";

export const SponsorHero = () => {
  return (
    <div>
      <Card className="p-0 bg-FFFFFF">
        <Card className="border-0">
          <Card.Body className="p-0">
            <div className={`${SponsorHeroStyle.hero_card}`}>
              <div className="flex-grow-1 p-3 p-lg-5">
                <EyebrowTitle
                  leftBorderColor="border_left_green"
                  title="NACS DAILY NEWS"
                />
                <div className="d-flex">
                  <div className="d-flex flex-column text-start pe-3 fs-5 color-3C5567">
                    <div>October</div>
                    <div>2024</div>
                  </div>
                  <div className="fs-1 fw-semibold ">
                    <div className="color-E54800">07</div>
                  </div>
                </div>
                <div className="text-start">
                  <p
                    className={`fw-semibold text-start m-0 mt-2 color-002569 ${SponsorHeroStyle.sponsor_title}`}
                  >
                    NACS Day On The Hill
                  </p>
                  <p className="m-0 mt-3 fw-light text-secondary fs-5 color-3C5567">
                    Sponsored By:
                    <div>
                      <img src={Wawa} alt="" />
                    </div>
                  </p>
                </div>
                <div className={`mt-4 d-flex ${SponsorHeroStyle.btn_section}`}>
                  <div className="mt-3 float-start">
                    <SideRoundButton
                      name={"Register Now"}
                      ficonType="fa-solid"
                      ficonName="fa-circle-dollar"
                      ficonSize="fa-xl"
                      className="mb-4"
                    />
                  </div>
                  <div className="mt-3 d-flex justify-content-between">
                    <InvertedButton
                      name="Add To Calendar"
                      siconType="fa-solid"
                      siconName="fa-plus"
                      ficonType="fa-solid"
                      ficonName="fa-calendar-circle-plus"
                    />
                  </div>
                </div>
              </div>
              <div className={`${SponsorHeroStyle.img_section}`}>
                <img src={clip} width={689} alt="" className="ms-5" />
                <div className={`${SponsorHeroStyle.c_shape_container}`}>
                  <video
                    className={`${SponsorHeroStyle.c_shaped_video}`}
                    autoPlay
                    loop
                    muted
                  >
                    <source
                      src="https://www.shutterstock.com/shutterstock/videos/1096130427/preview/stock-footage-time-lapse-of-modern-city-and-road-aerial-view.webm"
                      type="video/mp4"
                    />
                  </video>
                  <div className={`${SponsorHeroStyle.c_shape_cutout}`}></div>
                  <div
                    className={`${SponsorHeroStyle.rect_shape_cutout}`}
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
