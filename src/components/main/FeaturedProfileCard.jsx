import React from "react";
import { Card } from "react-bootstrap";
import FeaturedProfileCardStyle from "../../stylesheets/FeaturedProfileCardStyle.module.css";
import ProfileImage from "../../assests/images/Vector.png";
import { TextLinkButton } from "../ui-components/TextLinkButton";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";
import { Avatars } from "../ui-components/Avatars";

export const FeaturedProfileCard = () => {
  return (
    <div>
      <Card className={`border-0 p-3`}>
        <Card
          className={`border-0 p-0 bg-E9F0F4 ${FeaturedProfileCardStyle.section_card}`}
        >
          <Card.Body
            className={`p-4 ${FeaturedProfileCardStyle.section_body_card}`}
          >
            <div className="text-start mb-3 d-flex justify-content-between">
              <EyebrowTitle
                leftBorderColor="border_left_green"
                title="DESKTOP EYEBROW"
                titleColor="color-0053A5"
              />
              <Avatars size={70} imgSrc={ProfileImage} />
            </div>
            <div className="text-start">
              <p className=" m-0 fw-semibold fs-5 color-262d61">
                Christopher Wise
              </p>
              <p className="m-0 fs-5 color-262d61 mb-2">
                Research & Education Solutions Coordinator, NACS
              </p>
              <p className="m-0 mb-1 ps-2">
                <span className="pe-3">
                  <i className="fa-solid fa-phone color-0053A5"></i>
                </span>
                <span className="pe-3 color-0053A5">1234556789</span>
              </p>
              <p className="m-0 mb-2 ps-2">
                <span className="pe-3">
                  <i className="fa-solid fa-envelope color-0053A5"></i>
                </span>
                <span className="pe-3 color-0053A5">abc@xyz.com</span>
              </p>
              <p className="fs-5 color-262d61">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do
                eiusmod tempor incididunt ut labore et dolore magna aliqua.
              </p>
              <TextLinkButton name={"Learn More"} />
            </div>
          </Card.Body>
        </Card>
      </Card>
    </div>
  );
};
