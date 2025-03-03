import React from "react";
import { Card } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";
import EventCardStyle from "../../stylesheets/EventCardStyle.module.css";
import { InvertedButton } from "../ui-components/InvertedButton";
import ProfileImage from "../../assests/images/Vector.png";
import { Avatars } from "../ui-components/Avatars";

export const EventCard = () => {
  return (
    <div>
      <Card className={`p-0 mb-4 border-0 ${EventCardStyle.main_card}`}>
        <Card className={`border-0 ${EventCardStyle.section_card}`}>
          <Card.Body className="p-4">
            <div className="p-1 text-start fs-4">
              <span className="color-FFFFFF">
                From Swipe to Chip: Getting Ready for the EBT Chip Card
                Migration
              </span>
            </div>
            <div className="text-start">
              <div className="fs-5 mt-3">
                <i className={`fa-solid fa-calendar-days color-FFFFFF`}></i>
                <span className="ms-2 color-FFFFFF">Jul 23, 2024 4:00 PM</span>
              </div>
              <div className="fs-5 mt-3">
                <i className={`fa-solid fa-location-dot color-FFFFFF`}></i>
                <span className="ms-2 color-FFFFFF">Webinar</span>
              </div>
            </div>
            <div className="d-flex justify-content-between mt-3">
              <div className="fs-5 mt-2 mb-2">
                <div className={`${EventCardStyle.images_wrapper}`}>
                  <div>
                    {/* <img src={ProfileImage} alt="image 1" /> */}
                    <Avatars size={80} imgSrc={ProfileImage} />
                  </div>
                  <div>
                    {/* <img src={ProfileImage} alt="image 2" /> */}
                    <Avatars size={80} imgSrc={ProfileImage} />
                  </div>
                  <div>
                    <span className="pt-1 color-FFFFFF">+55</span>
                  </div>
                </div>
              </div>
              <div className="align-self-end">
                <InvertedButton
                  siconType="fa-solid"
                  siconName="fa-square-plus"
                  siconSize="fa-lg"
                  fontSize="fs-5"
                  name="Add to Calendar"
                />
              </div>
            </div>
          </Card.Body>
        </Card>
        {/* <div className="mt-5">
          <BackToLog />
        </div> */}
      </Card>
    </div>
  );
};
