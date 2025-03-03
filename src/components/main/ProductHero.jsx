import React, { useEffect, useRef, useState } from "react";
import { BackToLog } from "../ui-components/BackToLog";
import { Card, Col, Form, Row } from "react-bootstrap";
import { Breadcrumbs } from "../ui-components/Breadcrumbs";
import CLogoGreen from "../../assests/images/c-logo-green.png";
import ProductHeroImage from "../../assests/images/product-hero.png";
import ProductHeroImage1 from "../../assests/images/product-hero-1.png";
import ProductHeroImage2 from "../../assests/images/tier1hero.png";
import clip from "../../assests/images/Clip path group.png";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import ProductHeroStyle from "../../stylesheets/ProductHeroStyle.module.css";
import { SideRoundButton } from "../ui-components/SideRoundButton";
import { GreyButton } from "../ui-components/GreyButton";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";
import { Accordion } from "../ui-components/Accordion";
import { BioCard } from "./BioCard";
import { Footer } from "./Footer";
import { RecommendedCardCarousel } from "./RecommendedCardCarousel";
import { ShareIcon } from "../ui-components/ShareIcon";

const page = ["NACS", "NACS Products & Services", "Online Store", "Products"];

const data = [
  {
    id: 1,
    content: ProductHeroImage,
    desc: "Podcast",
  },
  {
    id: 2,
    content: ProductHeroImage1,
    desc: "Fuel",
  },
  {
    id: 3,
    content: ProductHeroImage2,
    desc: "Media",
  },
];

export const ProductHero = () => {
  const settings = {
    dots: false,
    infinite: true,
    speed: 500,
    slidesToShow: 3,
    slidesToScroll: 1,
  };
  let containerRef = useRef(null);
  const [currElement, setCurrElement] = useState(0);

  const next = () => {
    let count = containerRef.innerSlider.track.props.currentSlide;
    containerRef.slickNext();
    setCurrElement(currElement < data.length - 1 ? count + 1 : 0);
  };
  const previous = () => {
    let count = containerRef.innerSlider.track.props.currentSlide;
    containerRef.slickPrev();
    setCurrElement(currElement > 0 ? count - 1 : data.length - 1);
  };

  useEffect(() => {
    const element = document.getElementsByClassName("slick-arrow");
    const elementsArray = [...element];

    elementsArray.forEach((element) => {
      element.style.display = "none";
    });
  });

  return (
    <div>
      <Card className="position-relative">
        <img
          src={clip}
          width={500}
          alt=""
          className="z-1 position-absolute top-0 end-0 d-none d-lg-block"
        />
        <Card className="border-0 p-3">
          <Card.Body className={`p-0 pe-3 border-0`}>
            <Breadcrumbs pages={page} />
            <p className="text-secondary text-start">
              NACS State of the Industry Report of 2023 Data
            </p>
            <EyebrowTitle
              leftBorderColor="border_left_green"
              title="COOL NEW PRODUCT"
            />
            <div className="d-flex mt-5 mb-4">
              <img src={CLogoGreen} alt="" height={40} />
              <div>
                <p className="fs-3 fw-semibold ps-3 pe-3 text-start color-002569">
                  NACS State of the Industry Report of 2023 Data - Digital
                  License
                </p>
              </div>
            </div>
            <Row>
              <Col lg={6} md={6} sm={12} xs={12}>
                <div className={`${ProductHeroStyle.product_current_image}`}>
                  <img src={data[currElement].content} alt="" />
                </div>
                <Row className="pe-3 mt-4">
                  <Col lg={1} md={1} sm={1} xs={1}>
                    <i
                      onClick={previous}
                      className="fa-regular fa-angle-left fa-xl mt-5 pointer color-868F98"
                    ></i>
                  </Col>
                  <Col lg={10} md={10} sm={10} xs={10}>
                    <div className="slider-container">
                      <Slider
                        {...settings}
                        ref={(slider) => {
                          containerRef = slider;
                        }}
                      >
                        {data.map((item, index) => (
                          <div
                            key={index}
                            className={`${ProductHeroStyle.product_image}`}
                          >
                            <img src={item.content} alt="" />
                          </div>
                        ))}
                      </Slider>
                    </div>
                  </Col>
                  <Col lg={1} md={1} sm={1} xs={1}>
                    <i
                      className="fa-regular fa-angle-right fa-xl mt-5 pointer color-868F98"
                      onClick={next}
                    ></i>
                  </Col>
                </Row>
              </Col>
              <Col lg={6} md={6} sm={12} xs={12}>
                <div>
                  <p className="text-start fs-5">
                    Phasellus aliquam lacus non eros mollis convallis. Curabitur
                    porttitor sapien hendrerit, tempus libero lobortis,
                    imperdiet mi. Nam id rutrum nulla. Integer tincidunt dapibus
                    vehicula. Vestibulum varius tincidunt urna, ac efficitur
                    eros egestas at. Curabitur viverra tortor vel ante lacinia
                    tincidunt. Donec viverra odio nisi, ac venenatis diam
                    facilisis eu. Sed vestibulum sapien ante, eu euismod felis
                    condimentum eu. Donec sed vehicula nisl. Vestibulum aliquam
                    posuere est. Fusce rutrum est et posuere gravida. Sed
                    varius, turpis eget scelerisque mattis, diam eros blandit
                    leo, ac iaculis lectus leo nec ligula. Vivamus dapibus
                    interdum risus, vel pulvinar sapien pulvinar nec.
                  </p>
                </div>
                <div>
                  <Form.Select
                    aria-label="Default select example"
                    className="border_blue color-0053A5 fw-semibold"
                  >
                    <option>Open this select menu</option>
                    <option value="1">One</option>
                    <option value="2">Two</option>
                    <option value="3">Three</option>
                  </Form.Select>
                </div>
                <div className="text-start mt-4 fs-5">
                  <p className="p-0">
                    <span className="text-secondary pe-1">List Price:</span>
                    <span className="color-002569">$1,119.00</span>
                  </p>
                  <p className="p-0">
                    <span className="text-secondary pe-1">Member Price:</span>
                    <span className="color-002569">$1,119.00</span>
                  </p>
                  <p className="p-0">
                    <span className="text-secondary pe-1">
                      Additional Licenses-
                    </span>
                    <span className="text-secondary">
                      Lorem ipsum dolor sit amet, consectetur adipiscing elit.
                      Praesent bibendum velit non sapien gravida, nec vulputate
                      lectus feugiat.
                    </span>
                  </p>
                </div>
                <div className="d-flex flex-column flex-lg-row mt-4">
                  <SideRoundButton
                    name="Purchase Now"
                    siconName="fa-arrow-right-long"
                    btnClass="w-100 d-inline-block"
                    className="float-lg-start me-3"
                  />
                  <GreyButton
                    name="Become a Member"
                    siconType="fa-regular"
                    siconName="fa-arrow-right-long"
                    btnClass="w-100 mt-lg-0 mt-22 d-inline-block"
                    className="float-lg-start"
                  />
                </div>
              </Col>
            </Row>
          </Card.Body>
          <Card.Body className="mt-4">
            <Row>
              <Col lg={8} md={8} sm={12} xs={12}>
                <Accordion />
                <Col lg={6} md={6} sm={12} xs={12}>
                  <div className="mt-5">
                    <ShareIcon />
                  </div>
                </Col>
              </Col>
              <Col lg={4} md={4} sm={12} xs={12}>
                <BioCard />
              </Col>
            </Row>
          </Card.Body>
          <Card.Body>
            <RecommendedCardCarousel />
          </Card.Body>
        </Card>
        <div className="mt-5">
          <Footer />
        </div>
      </Card>
    </div>
  );
};
