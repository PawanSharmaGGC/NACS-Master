import React, { useRef, useState } from "react";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import DeepDiveImage from "../../assests/images/deep-dive.png";
import CLogoLight from "../../assests/images/c-logo-light.png";
import { Card, Nav } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";
import DeepDiveStyle from "../../stylesheets/DeepDiveStyle.module.css";
import CLogoGreen from "../../assests/images/c-logo-green.png";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";

const data = [
  {
    id: 1,
    text: "State Of Fuels.",
    House: "Media",
  },
  {
    id: 2,
    text: "Goya Introduces Chickpea Puffs..",
    House: "Fuel",
  },
  {
    id: 3,
    text: "State Of Fuels.",
    House: "Media",
  },
  {
    id: 4,
    text: "Goya Introduces Chickpea Puffs..",
    House: "Fuel",
  },
  {
    id: 5,
    text: "State Of Fuels.",
    House: "Media",
  },
  {
    id: 6,
    text: "Goya Introduces Chickpea Puffs..",
    House: "Fuel",
  },
  {
    id: 7,
    text: "State Of Fuels.",
    House: "Media",
  },
  {
    id: 8,
    text: "Goya Introduces Chickpea Puffs..",
    House: "Fuel",
  },
];

export const DeepDive = () => {
  const settings = {
    className: "center",
    centerMode: true,
    infinite: true,
    centerPadding: "60px",
    slidesToShow: 3,
    speed: 500,
    responsive: [
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1,
        },
      },
    ],
  };
  let containerRef = useRef(null);
  const [topElement, setTopElement] = useState(0);
  const [prevElement, setPrevElement] = useState(data.length - 1);
  const [nextElement, setNextElement] = useState(1);

  const next = () => {
    let count = containerRef.innerSlider.track.props.currentSlide;
    containerRef.slickNext();
    setTopElement(topElement < data.length - 1 ? count + 1 : 0);
    setPrevElement(topElement < data.length - 1 ? count : data.length - 1);
    setNextElement(
      count + 1 == data.length - 1
        ? 0
        : count == data.length - 1
        ? 1
        : count + 2
    );
  };
  const previous = () => {
    let count = containerRef.innerSlider.track.props.currentSlide;
    containerRef.slickPrev();
    setTopElement(topElement > 0 ? count - 1 : data.length - 1);
    setPrevElement(
      count == 0 ? data.length - 2 : count == 1 ? data.length - 1 : count - 2
    );
    setNextElement(count == 0 ? 0 : count);
  };
  return (
    <div>
      <Card className={`${DeepDiveStyle.main_card}`}>
        <Card className="border-0 p-2">
          <Card.Body
            className={`p-0 border-0 ${DeepDiveStyle.section_card_body}`}
          >
            <div>
              <div>
                <img
                  fluid
                  src={DeepDiveImage}
                  alt=""
                  className={`${DeepDiveStyle.header_img}`}
                />
              </div>
            </div>
            <img
              src={CLogoLight}
              alt=""
              className={`${DeepDiveStyle.c_logo_light}`}
            />
            <div className={`position-absolute ${DeepDiveStyle.title_card}`}>
              <div class="d-flex flex-column mt-lg-3 mt-2 ps-lg-5 ps-3">
                <div class="p-3 mt-lg-3 text-light fs-5 text-start">
                  Dive Deeper
                </div>
                <div class="p-3">
                  <Nav variant="pills" defaultActiveKey="link-1">
                    <Nav.Item className="me-2">
                      <Nav.Link
                        className={`${DeepDiveStyle.dive_nav_link}`}
                        eventKey="link-1"
                      >
                        Trends
                      </Nav.Link>
                    </Nav.Item>
                    <Nav.Item className="me-2">
                      <Nav.Link
                        className={`${DeepDiveStyle.dive_nav_link}`}
                        eventKey="link-2"
                      >
                        Analytics
                      </Nav.Link>
                    </Nav.Item>
                    <Nav.Item className="me-2">
                      <Nav.Link
                        className={`${DeepDiveStyle.dive_nav_link}`}
                        eventKey="link-3"
                      >
                        Policies
                      </Nav.Link>
                    </Nav.Item>
                  </Nav>
                </div>
              </div>

              <div>
                <Card className="p-0 border-0 bg-trans">
                  <div className={`bg-trans ${DeepDiveStyle.slider_container}`}>
                    <Slider
                      {...settings}
                      ref={(slider) => {
                        containerRef = slider;
                      }}
                    >
                      {data.map((item, index) => (
                        <Card
                          className={
                            DeepDiveStyle["flex-card"] + " border border-0"
                          }
                          key={index}
                        >
                          <Card.Body
                            className={`p-4 mb-3 ${
                              topElement == index
                                ? DeepDiveStyle.slider_active_card_body
                                : DeepDiveStyle.slider_card_body
                            } ${
                              topElement == index ||
                              nextElement === index ||
                              prevElement === index
                                ? "bg-FFFFFF"
                                : "bg-296DC1"
                            }`}
                          >
                            <>
                              <EyebrowTitle
                                leftBorderColor="border_left_blue"
                                title="NACS DAILY NEWS"
                              />
                              <div
                                className={`p-1 text-start ${
                                  topElement === index ? "fs-3" : "fs-5"
                                }`}
                              >
                                <span className="color-0053A5">
                                  10 Convinient Store Facts That WIll Make You
                                  Rethink Your Current Business Methods
                                </span>
                              </div>
                              <div className="d-flex justify-content-between">
                                <div className="fs-5 mt-3">
                                  <span className="me-2 color-0053A5">
                                    Read Story
                                  </span>
                                  <i className="fa-regular fa-arrow-right color-0053A5"></i>
                                </div>
                                <div className="align-self-end opacity-50">
                                  <img src={CLogoGreen} alt="CLogoGreen" />
                                </div>
                              </div>
                            </>
                          </Card.Body>
                        </Card>
                      ))}
                    </Slider>
                    <div className="d-flex justify-content-center">
                      <p
                        className={`pointer ${
                          topElement === 0 ? "color-868F98" : "color-FFFFFF"
                        }`}
                        onClick={topElement === 0 ? null : previous}
                      >
                        <i className="fa-regular fa-angle-left fa-xl"></i>
                      </p>
                      <span className="pe-4"></span>
                      <p
                        className={`pointer ${
                          topElement === data.length - 1
                            ? "color-868F98"
                            : "color-FFFFFF"
                        }`}
                        onClick={topElement === data.length - 1 ? null : next}
                      >
                        <i className="fa-regular fa-angle-right fa-xl "></i>
                      </p>
                    </div>
                  </div>
                </Card>
              </div>
            </div>
          </Card.Body>
        </Card>
        <div className="mt-5">
          <BackToLog />
        </div>
      </Card>
    </div>
  );
};
