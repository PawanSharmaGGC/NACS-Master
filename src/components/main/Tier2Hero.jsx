import React from "react";
import Tier2 from "../../assests/images/tier-2.png";
import { Card, Col, Row } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";
import Tier2HeroStyle from "../../stylesheets/Tier2HeroStyle.module.css";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";
import { FeaturedContentCard } from "./FeaturedContentCard";
import { EventSearchSortFilter } from "../ui-components/EventSearchSortFilter";
import { InlineRelatedPost } from "./InlineRelatedPost";
import { RecommendedCardCarousel } from "./RecommendedCardCarousel";
import { FAQCard } from "./FAQCard";
import { BioCard } from "./BioCard";
import { Footer } from "./Footer";
import { SearchComponent } from "../ui-components/SearchComponent";

export const Tier2Hero = () => {
  return (
    <div>
      <Card className={`p-3 ${Tier2HeroStyle.main_card}`}>
        <div>
          <EyebrowTitle
            title="NACS DAILY NEWS"
            leftBorderColor="border_left_green"
          />
          <div className="height_16_rem">
            <div className="position-relative">
              <img
                src={Tier2}
                // src={Highway}
                alt="Highway Interchange"
                className={` ${Tier2HeroStyle.img_highway_interchange}`}
                // width={320}
              />
              <div
                className={`text-start position-absolute fw-bolder ${Tier2HeroStyle.overlay_txt_img_pos}`}
              >
                <span className={`${Tier2HeroStyle.overlay_txt_img}`}>
                  NACS Joins Responsibility Commitment For Crossover Alcohol
                  Products
                </span>
              </div>
            </div>
          </div>
          <div className="text-start mt-sm-2 mt-lg-5 pt-lg-5 color-262d61">
            <p className="fs-4 d-lg-none d-sm-block d-xl-none">
              Four Leading Associations Join Forces.
            </p>
            <p className="fs-5 pt-lg-4">
              <span>24 March 2024</span> |{" "}
              <span className="text-body-tertiary">5min Read</span>
            </p>
          </div>
        </div>
        <div>
          <FeaturedContentCard />
        </div>
        <div>
          <SearchComponent />
        </div>
        <div>
          <InlineRelatedPost />
        </div>
        <div>
          <RecommendedCardCarousel />
        </div>
        <Row>
          <Col lg={8} sm={12}>
            <FAQCard />
          </Col>
          <Col lg={4} sm={12}>
            <BioCard />
          </Col>
        </Row>
        <div className="mt-4">
          <Footer />
        </div>
      </Card>
    </div>
  );
};
