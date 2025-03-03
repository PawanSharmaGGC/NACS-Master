import React, { useState } from "react";
import ExpandableContentCardStyle from "../../stylesheets/ExpandableContentCardStyle.module.css";
import ContentImage from "../../assests/images/content-img.png";
import { Card } from "react-bootstrap";
import { SideRoundButton } from "../ui-components/SideRoundButton";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";

export const ExpandableContentCard = () => {
  const [isOpen, setIsOpen] = useState(false);

  const toggleCollapse = () => {
    setIsOpen(!isOpen);
  };

  return (
    <div>
      <Card className={`border-0 p-3`}>
        <Card
          className={`border-0 p-0 ${ExpandableContentCardStyle.section_card}`}
        >
          <Card.Body
            className={`bg-0053A5 ${ExpandableContentCardStyle.section_body_card}`}
          >
            <div className="text-start mb-2 d-flex flex-column">
              <img
                src={ContentImage}
                className={`img-fluid ${ExpandableContentCardStyle.section_body_img}`}
                alt="content-image"
              />
              <div className="p-4 text-start color-FFFFFF">
                <EyebrowTitle
                  leftBorderColor="border_left_white"
                  title="FUEL"
                  titleColor="color-FFFFFF"
                />
                <p className="m-0 fs-4 mb-2">
                  Energy Provider: Fuels And Electrification
                </p>
                <p className="m-0 fs-5 mb-2">
                  U.S. convenience stores sell approximately 80% of the gasoline
                  purchased in the country. As more choices come to market,
                  c-stores meet the needs of nearly 40 million drivers each day.
                </p>
              </div>
            </div>
          </Card.Body>
          <Card.Body
            className={`${ExpandableContentCardStyle.expandable_card_body}`}
          >
            <div
              className={`bg-E9F0F4 ${ExpandableContentCardStyle.expandable_card}`}
            >
              {/* Desktop Version */}
              <div className="d-lg-block d-none ">
                <div className="d-flex p-3 text-start">
                  <div
                    className={` ${ExpandableContentCardStyle.content_badge}`}
                  >
                    <i className="fa-thin fa-charging-station color-FFFFFF fa-xl pt-3 ps-2"></i>
                  </div>
                  <div className="ps-3">
                    <p className="m-0">
                      <span className="pe-2 fs-5 color-0053A5">
                        Electric Vehical
                      </span>
                      <i className="fa-regular fa-arrow-right color-0053A5"></i>
                    </p>
                    <p className="color-3C5567 fs-5">
                      Explore whether to invest in EV infrastructure when its
                      right for you.
                    </p>
                  </div>
                </div>
                <div className="d-flex p-3 text-start">
                  <div
                    className={` ${ExpandableContentCardStyle.content_badge}`}
                  >
                    <i className="fa-thin fa-car-side color-FFFFFF fa-xl pt-3 ps-2"></i>
                  </div>
                  <div className="ps-3">
                    <p className="m-0">
                      <span className="fs-5 color-0053A5 pe-2">
                        Fuel Resource Center
                      </span>
                      <i className="fa-regular fa-arrow-right color-0053A5"></i>
                    </p>
                    <p className="color-3C5567 fs-5">
                      An insider's view of how fuel is ultimately sold at
                      c-stores.
                    </p>
                  </div>
                </div>
              </div>
              {/* Mobile version */}
              <div className="d-sm-block d-lg-none d-xl-none d-md-none p-3 pt-4 text-start">
                <SideRoundButton
                  handleClick={toggleCollapse}
                  name={isOpen ? "Close" : "Expand"}
                  siconType="fa-solid"
                  siconName={isOpen ? "fa-circle-xmark" : "fa-square-plus"}
                  className="mt-4 mb-3"
                />
                <div
                  className={`collapse ${
                    ExpandableContentCardStyle.collapse_section
                  } ${isOpen ? "show" : ""}`}
                  id="collapseExample"
                >
                  <div className="d-flex p-3 text-start">
                    <div
                      className={`${ExpandableContentCardStyle.content_badge}`}
                    >
                      <i className="fa-thin fa-piggy-bank fa-xl color-FFFFFF pt-4 ps-2"></i>
                    </div>
                    <div className="ps-3">
                      <p className="color-3C5567 fs-5">
                        An insider's view of how fuel is ultimately sold at
                        c-stores.
                      </p>
                    </div>
                  </div>
                  <div className="d-flex p-3 text-start">
                    <div
                      className={` ${ExpandableContentCardStyle.content_badge}`}
                    >
                      <i className="fa-thin fa-piggy-bank fa-xl color-FFFFFF pt-4 ps-2"></i>
                    </div>
                    <div className="ps-3">
                      <p className="color-3C5567 fs-5">
                        An insider's view of how fuel is ultimately sold at
                        c-stores.
                      </p>
                    </div>
                  </div>
                  <div className="d-flex p-3 text-start">
                    <div
                      className={` ${ExpandableContentCardStyle.content_badge}`}
                    >
                      <i className="fa-thin fa-piggy-bank fa-xl color-FFFFFF pt-4 ps-2"></i>
                    </div>
                    <div className="ps-3">
                      <p className="color-3C5567 fs-5">
                        An insider's view of how fuel is ultimately sold at
                        c-stores.
                      </p>
                    </div>
                  </div>
                  <div className="d-flex p-3 text-start">
                    <div
                      className={` ${ExpandableContentCardStyle.content_badge}`}
                    >
                      <i className="fa-thin fa-piggy-bank fa-xl color-FFFFFF pt-4 ps-2"></i>
                    </div>
                    <div className="ps-3">
                      <p className="color-3C5567 fs-5">
                        An insider's view of how fuel is ultimately sold at
                        c-stores.
                      </p>
                    </div>
                  </div>
                  <div className="d-flex p-3 text-start">
                    <div
                      className={` ${ExpandableContentCardStyle.content_badge}`}
                    >
                      <i className="fa-thin fa-piggy-bank fa-xl color-FFFFFF pt-4 ps-2"></i>
                    </div>
                    <div className="ps-3">
                      <p className="color-3C5567 fs-5">
                        An insider's view of how fuel is ultimately sold at
                        c-stores.
                      </p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </Card.Body>
        </Card>
      </Card>
    </div>
  );
};
