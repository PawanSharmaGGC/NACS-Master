import React, { useEffect, useRef, useState } from "react";
import { Card, Col, Row } from "react-bootstrap";
import clip from "../../assests/images/Clip path group.png";
import Tier1Cover from "../../assests/images/tier1hero.png";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import Tier1HeroStyle from "../../stylesheets/Tier1HeroStyle.module.css";
import { SideRoundButton } from "../ui-components/SideRoundButton";
import { Breadcrumbs } from "../ui-components/Breadcrumbs";
import { BackToLog } from "../ui-components/BackToLog";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";
import { Tag } from "../ui-components/Tag";
import { TextLinkButton } from "../ui-components/TextLinkButton";

const data = [
  {
    id: 1,
    text: "It’s 24/7 day: Here’s how retailers are celebrating hometown heroes.",
    House: "Media",
  },
  {
    id: 2,
    text: "Who are tomorrow’s leaders?",
    House: "Fuel",
  },
  {
    id: 3,
    text: "State of Fuels and Electrification",
    House: "Media",
  },
  {
    id: 4,
    text: "An alternative to electrification: Low carbon liquid fuels",
    House: "Fuel",
  },
];

const page = ["Home"];

export const Tier1Hero = () => {
  const settings = {
    className: "center",
    centerMode: true,
    infinite: true,
    centerPadding: "60px",
    slidesToShow: 1,
    speed: 500,
  };
  let containerRef = useRef(null);
  const [topElement, setTopElement] = useState(0);

  const next = () => {
    let count = containerRef.innerSlider.track.props.currentSlide;
    containerRef.slickNext();
    setTopElement(topElement < data.length - 1 ? count + 1 : 0);
  };
  const previous = () => {
    let count = containerRef.innerSlider.track.props.currentSlide;
    containerRef.slickPrev();
    setTopElement(topElement > 0 ? count - 1 : data.length - 1);
  };

  useEffect(() => {
    const arrowElement = document.getElementsByClassName("slick-arrow");
    const arrowElementsArray = [...arrowElement]; // Convert to array
    arrowElementsArray.forEach((element) => {
      console.log(element); // Do something with each element
      element.style.display = "none";
    });
  });
  return (
    <div>
      <Card className={`${Tier1HeroStyle.main_card}`}>
        <img
          className={`ml-50-per ${Tier1HeroStyle.card_img}`}
          src={clip}
          alt=""
        />
        <div className="position-absolute m-4">
          <Breadcrumbs pages={page} />
          <EyebrowTitle
            title="WELCOME TO CONVENIENCE.ORG"
            leftBorderColor="border_left_green"
            titleColor="color-FFFFFF"
          />
        </div>
        <Card
          className={`border-0 p-0 position-absolute bg-trans ${Tier1HeroStyle.section_card}`}
        >
          <Card.Body
            className={`p-4 flex-fill ${Tier1HeroStyle.section_body_card}`}
          >
            <div className="position-relative float-lg-start">
              <div className={` ${Tier1HeroStyle.image_container}`}>
                <div className={` ${Tier1HeroStyle.blur_overlay}`}></div>
                <img
                  className={` ${Tier1HeroStyle.hero_card}`}
                  src={Tier1Cover}
                  width="341px"
                  alt=""
                />
              </div>
              <div
                className={`position-absolute text-light ${Tier1HeroStyle.txt_over_img}`}
              >
                <EyebrowTitle
                  title="ARTICLE"
                  leftBorderColor="border_left_white"
                  titleColor="color-FFFFFF"
                />
                <div className=" text-start fw-semibold fs-3 mb-3">
                  <span>
                    Stopping Swipe Fees On State Taxes: Illinois's Landmark Law
                  </span>
                </div>
                <div className="d-flex justify-content-between">
                  <p className="mb-0">
                    <span className="me-3">24 March 2024</span>
                    <span className="me-3"> | </span>
                    <span className="">03:00 PM Eastern Time</span>
                  </p>
                </div>
                <div className="mt-3">
                  <SideRoundButton
                    className="float-start"
                    name="Read Articale"
                  />
                </div>
              </div>
            </div>
          </Card.Body>
          <Card.Body className={`p-0 border-0 bg-trans flex-fill flex-grow-1`}>
            <div>
              <Card
                className={`p-0 border-0 bg-trans ${Tier1HeroStyle.slider_card}`}
              >
                {/*.......................Mobile View............................*/}
                <div
                  className={`p-0 border-0 bg-trans slider-container ${Tier1HeroStyle.slider}`}
                >
                  <Slider
                    {...settings}
                    ref={(slider) => {
                      containerRef = slider;
                    }}
                  >
                    {data.map((item, index) => (
                      <Card
                        className={`border border-0 bg-trans ${Tier1HeroStyle.flex_card} ${Tier1HeroStyle.slider_card}`}
                        key={index}
                      >
                        <Card.Body
                          className={`m-3 brdr-rad-18 ${
                            topElement == index ? "bg-DBEAB9" : "bg-FFFFFF"
                          }`}
                        >
                          <div className="text-start p-2">
                            <div className="text-start align-items-baseline d-flex justify-content-between">
                              <EyebrowTitle
                                title={item.House}
                                leftBorderColor="border_left_green"
                                titleColor="color-0053A5"
                              />
                              <Tag
                                tagName="NEW"
                                bgColor="bg-DBEAB9"
                                textColor="color-0053A5"
                              />
                            </div>
                            <div className="fs-5 mb-2 color-0053A5">
                              {item.text}
                            </div>
                            <div className="color-0053A5">{item.writter}</div>
                            <div className="d-flex justify-content-between">
                              <TextLinkButton name="Read Story" />
                              <span>
                                <svg
                                  width="32"
                                  height="32"
                                  viewBox="0 0 32 32"
                                  fill="none"
                                  xmlns="http://www.w3.org/2000/svg"
                                >
                                  <path
                                    d="M32 16C32 7.16344 24.8366 0 16 0C7.16344 0 0 7.16344 0 16C0 24.8366 7.16344 32 16 32C24.8366 32 32 24.8366 32 16Z"
                                    fill="#85B0C6"
                                  />
                                  <path
                                    d="M9.33398 14.6669V17.3339C9.33398 18.6669 10.001 19.3339 11.334 19.3339H12.287C12.534 19.3339 12.781 19.4069 12.994 19.5339L14.941 20.7539C16.621 21.8069 18.001 21.0399 18.001 19.0599V12.9399C18.001 10.9539 16.621 10.1939 14.941 11.2469L12.994 12.4669C12.781 12.5939 12.534 12.6669 12.287 12.6669H11.334C10.001 12.6669 9.33398 13.3339 9.33398 14.6669Z"
                                    stroke="white"
                                    stroke-width="1.5"
                                  />
                                  <path
                                    d="M20 13.333C21.187 14.913 21.187 17.087 20 18.667"
                                    stroke="white"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                  />
                                  <path
                                    d="M21.2209 11.667C23.1469 14.233 23.1469 17.767 21.2209 20.333"
                                    stroke="white"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                  />
                                </svg>
                              </span>
                            </div>
                          </div>
                        </Card.Body>
                      </Card>
                    ))}
                  </Slider>
                  <div className="text-center mt-4">
                    <i
                      onClick={previous}
                      className="fa-regular fa-angle-left fa-lg color-FFFFFF"
                    ></i>
                    <span className="pe-5"></span>
                    <i
                      onClick={next}
                      className="fa-regular fa-angle-right fa-lg color-FFFFFF"
                    ></i>
                  </div>
                </div>

                {/*.......................Desktop View............................*/}

                <Row className={`${Tier1HeroStyle.grid_view}`}>
                  {data.map((item, index) => (
                    <Col lg={6} md={6}>
                      <Card
                        className={`border border-0 bg-trans ${Tier1HeroStyle.flex_card}`}
                        key={index}
                      >
                        <Card.Body
                          className={`m-3 brdr-rad-18 ${
                            topElement == index ? "bg-DBEAB9" : "bg-FFFFFF"
                          }`}
                        >
                          <div className="text-start p-2">
                            <div className="text-start d-flex justify-content-between align-items-baseline">
                              <EyebrowTitle
                                title={item.House}
                                leftBorderColor="border_left_green"
                                titleColor="color-0053A5"
                              />
                              <Tag
                                tagName="NEW"
                                bgColor="bg-DBEAB9"
                                textColor="color-0053A5"
                              />
                            </div>
                            <div className="fs-5 mb-5 mt-4 color-0053A5">
                              {item.text}
                            </div>
                            <div className="color-0053A5">{item.writter}</div>
                            <div className="d-flex justify-content-between">
                              <TextLinkButton name="Read Story" />
                              <span>
                                <svg
                                  width="32"
                                  height="32"
                                  viewBox="0 0 32 32"
                                  fill="none"
                                  xmlns="http://www.w3.org/2000/svg"
                                >
                                  <path
                                    d="M32 16C32 7.16344 24.8366 0 16 0C7.16344 0 0 7.16344 0 16C0 24.8366 7.16344 32 16 32C24.8366 32 32 24.8366 32 16Z"
                                    fill="#85B0C6"
                                  />
                                  <path
                                    d="M9.33398 14.6669V17.3339C9.33398 18.6669 10.001 19.3339 11.334 19.3339H12.287C12.534 19.3339 12.781 19.4069 12.994 19.5339L14.941 20.7539C16.621 21.8069 18.001 21.0399 18.001 19.0599V12.9399C18.001 10.9539 16.621 10.1939 14.941 11.2469L12.994 12.4669C12.781 12.5939 12.534 12.6669 12.287 12.6669H11.334C10.001 12.6669 9.33398 13.3339 9.33398 14.6669Z"
                                    stroke="white"
                                    stroke-width="1.5"
                                  />
                                  <path
                                    d="M20 13.333C21.187 14.913 21.187 17.087 20 18.667"
                                    stroke="white"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                  />
                                  <path
                                    d="M21.2209 11.667C23.1469 14.233 23.1469 17.767 21.2209 20.333"
                                    stroke="white"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                  />
                                </svg>
                              </span>
                            </div>
                          </div>
                        </Card.Body>
                      </Card>
                    </Col>
                  ))}
                </Row>
              </Card>
            </div>
          </Card.Body>
        </Card>
      </Card>
      <div className="mt-5 mb-3">
        <BackToLog />
      </div>
    </div>
  );
};
