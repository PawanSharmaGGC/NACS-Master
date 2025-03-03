import React from "react";
import FeacturedContentCardStyle from "../../stylesheets/FeaturedContentCardStyle.module.css";
import { Card } from "react-bootstrap";
import ContentImage from "../../assests/images/card-carousel.png";
import ProfileImage from "../../assests/images/Vector.png";
import { SideRoundButton } from "../ui-components/SideRoundButton";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";
import { Tag } from "../ui-components/Tag";
import { Avatars } from "../ui-components/Avatars";

export const FeaturedContentCard = () => {
  return (
    <div>
      <Card className={`border-0 p-3`}>
        <Card className={`p-0 ${FeacturedContentCardStyle.section_card}`}>
          <Card.Body className={`p-4`}>
            <div className="text-start align-items-baseline d-flex justify-content-between">
              <EyebrowTitle
                leftBorderColor="border_left_green"
                title="FEATURED"
                titleColor="color-0053A5"
              />
              <Tag tagName="NEW" bgColor="bg-DBEAB9" textColor="color-0053A5" />
            </div>
            <div
              className={`d-flex ${FeacturedContentCardStyle.section_body_card}`}
            >
              <div>
                <p className="fs-4 text-start color-0053A5 mb-4 d-lg-none d-sm-block">
                  Charging 101 Understanding The Basics
                </p>
                <img
                  src={ContentImage}
                  className={`image-fluid ${FeacturedContentCardStyle.content_image}`}
                  alt="content-image"
                />
              </div>
              <div className="ps-lg-5 text-start mt-5">
                <p className="fs-4 color-0053A5 mb-4 d-none d-lg-block">
                  Charging 101 Understanding The Basics
                </p>
                <p className="d-flex mb-4 fs-5">
                  <span className="pe-3 pt-3 pt-lg-0">
                    <Avatars size={40} imgSrc={ProfileImage} />
                  </span>
                  <span className="pe-3 color-0053A5">Amelia Grey</span>
                  <span className="pe-3 text-body-tertiary">May 15, 2024</span>
                  <span className="pe-3 text-body-tertiary">5min Read</span>
                </p>
                <p>
                  <SideRoundButton name="Read More" />
                </p>
              </div>
            </div>
          </Card.Body>
        </Card>
      </Card>
    </div>
  );
};
