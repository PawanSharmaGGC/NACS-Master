import React, { useRef, useState } from "react";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import { InvertedButton } from "../ui-components/InvertedButton";
import { SideRoundButton } from "../ui-components/SideRoundButton";
import { BackToLog } from "../ui-components/BackToLog";
import EventHeroStyle from "../../stylesheets/EventHeroStyle.module.css";
import { Card, CardBody, Col, Row } from "react-bootstrap";
import { VideoJS } from "../ui-components/VideoJs";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";
import { PaginationDot } from "../ui-components/PaginationDot";
import { PriceCard } from "./PriceCard";
import { EventDetails } from "./EventDetails";
import { TableComponent } from "../ui-components/TableComponent";
import { TabComponent } from "../ui-components/TabComponent";
import { EventCarousel } from "./EventCarousel";
import { Tier1ContentCard } from "./Tier1ContentCard";
import { EventCard } from "./EventCard";
import { Footer } from "./Footer";
import { ShareIcon } from "../ui-components/ShareIcon";

const data = [
  {
    id: 1,
    text: "Whats Going On With EV Sales? - Episode 347",
    House: "Podcast",
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

export const EventHero = () => {
  const videoJsOptions = {
    autoplay: false,
    controls: true,
    responsive: true,
    loop: true,
    muted: true,
    preload: "auto",
    fluid: true,
    poster: "",
    sources: [
      {
        src: "https://archive.org/download/BigBuckBunny_124/Content/big_buck_bunny_720p_surround.mp4",
        type: "video/mp4",
      },
    ],
  };
  const settings = {
    infinite: true,
    speed: 500,
    slidesToShow: 1,
    slidesToScroll: 1,
    responsive: [
      {
        breakpoint: 480,
        settings: {
          className: "center",
          centerMode: true,
          infinite: true,
          centerPadding: "60px",
          slidesToShow: 1,
          speed: 500,
        },
      },
    ],
  };
  let containerRef = useRef(null);
  const [currElement, setCurrElement] = useState(0);
  const [prevElement, setPrevElement] = useState(data.length - 1);
  const [nextElement, setNextElement] = useState(1);

  const next = () => {
    let count = containerRef.innerSlider.track.props.currentSlide;
    containerRef.slickNext();
    setCurrElement(currElement < data.length - 1 ? count + 1 : 0);
    setPrevElement(currElement < data.length - 1 ? count : data.length - 1);
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
    setCurrElement(currElement > 0 ? count - 1 : data.length - 1);
    setPrevElement(
      count == 0 ? data.length - 2 : count == 1 ? data.length - 1 : count - 2
    );
    setNextElement(count == 0 ? 0 : count);
  };
  return (
    <div>
      <Card className="p-0 bg-FFFFFF">
        <Card className="border-0 mb-3">
          <Card.Body className="p-0">
            <div className="p-3 ps-lg-5">
              <EyebrowTitle
                leftBorderColor="border_left_green"
                title="NACS Food Safety Forum"
              />
            </div>
            <div className={`${EventHeroStyle.hero_card}`}>
              <div className="flex-grow-1 p-3 p-lg-5">
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
                    className={`fw-semibold text-start m-0 mt-2 color-002569 ${EventHeroStyle.sponsor_title}`}
                  >
                    NACS Food Safety Forum
                  </p>
                  <p className="m-0 mt-3 fs-5 color-0053A5">
                    <span className="pe-3">
                      <i className="fa-solid fa-location-dot color-0053A5"></i>
                    </span>
                    <span>Las Vegas, Nevada, USA</span>
                  </p>
                </div>
                <div className={`mt-4 d-flex ${EventHeroStyle.btn_section}`}>
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
              <div className={`${EventHeroStyle.slider_container}`}>
                <Slider
                  {...settings}
                  ref={(slider) => {
                    containerRef = slider;
                  }}
                >
                  {data.map((item, index) => (
                    <Card key={index} className="border border-0">
                      <Card.Body>
                        <VideoJS
                          className={`brdr-rad-18`}
                          className1={`slider_video`}
                          options={videoJsOptions}
                        />
                      </Card.Body>
                    </Card>
                  ))}
                </Slider>
                <PaginationDot
                  currElement={currElement}
                  data={data}
                  next={next}
                  previous={previous}
                />
              </div>
            </div>
          </Card.Body>
          <Card.Body>
            <EventDetails />
          </Card.Body>
          <Card.Body>
            <Row className="border-0 p-3">
              <Col lg={8} md={8} sm={12} xs={12}>
                <TabComponent />
              </Col>
              <Col lg={4} md={4} sm={12} xs={12}>
                <PriceCard />
                <div className="mt-5">
                  <ShareIcon />
                </div>
              </Col>
            </Row>
          </Card.Body>
          <Card.Body>
            <EventCarousel />
          </Card.Body>
          <Card.Body>
            <p className="h3 color-002569 text-start mb-5">Related Events</p>
            <Row>
              <Col lg={4} md={4} sm={12} xs={12}>
                <Tier1ContentCard />
              </Col>
              <Col lg={8} md={8} sm={12} xs={12}>
                <Row>
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <EventCard />
                  </Col>
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <EventCard />
                  </Col>
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <EventCard />
                  </Col>
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <EventCard />
                  </Col>
                </Row>
              </Col>
            </Row>
          </Card.Body>
        </Card>
        <div className="mt-4">
          {/* <BackToLog /> */}
          <Footer />
        </div>
      </Card>
    </div>
  );
};
