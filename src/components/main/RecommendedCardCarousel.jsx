import React, { useRef, useState } from "react";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import CLogoGreen from "../../assests/images/c-logo-green.png";
import CardCarousel from "../../assests/images/card-carousel.png";
import { Card } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";
import RecommendedCardCarouselStyle from "../../stylesheets/RecommendedCardCarouselStyle.module.css";
import { SideRoundButton } from "../ui-components/SideRoundButton";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";

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

export const RecommendedCardCarousel = () => {
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
      <Card className="p-0 border-0 bg-trans">
        <div className="d-flex justify-content-between ps-lg-5 pe-lg-5 ps-0 pe-0">
          <div className="fs-3 color-0053A5">You May Also Like</div>
          <div>
            <SideRoundButton
              name="Button Desktop"
              btnClass="w-100 d-inline-block"
            />
          </div>
        </div>
        <div
          className={`bg-trans ${RecommendedCardCarouselStyle.slider_container}`}
        >
          <Slider
            {...settings}
            ref={(slider) => {
              containerRef = slider;
            }}
          >
            {data.map((item, index) => (
              <Card
                className={`${RecommendedCardCarouselStyle.flex_card} ${
                  currElement == index &&
                  RecommendedCardCarouselStyle.curr_slider_card
                } ${
                  prevElement == index &&
                  RecommendedCardCarouselStyle.prev_slider_card
                } ${
                  nextElement == index &&
                  RecommendedCardCarouselStyle.next_slider_card
                } ${
                  nextElement !== index &&
                  currElement !== index &&
                  prevElement !== index &&
                  RecommendedCardCarouselStyle.slider_card
                } border border-0`}
                key={index}
              >
                <Card.Body
                  className={`bg-DBEAB9 m-3 brdr-rad-18 ${RecommendedCardCarouselStyle.slider_card_body} `}
                >
                  <>
                    <EyebrowTitle
                      leftBorderColor="border_left_blue"
                      title={item.House}
                    />

                    <div className="p-1 text-start fs-4">
                      <span className="color-0053A5">{item.text}</span>
                    </div>
                    <div className="">
                      <img
                        src={CardCarousel}
                        className="img-fluid"
                        alt="card-carousel"
                      />
                      <div className="d-flex align-items-end flex-column">
                        <img
                          src={CLogoGreen}
                          className="img-fluid pt-3"
                          alt="c-logo-green"
                        />
                      </div>
                    </div>
                  </>
                </Card.Body>
              </Card>
            ))}
          </Slider>
          <div className="text-center">
            <i
              onClick={previous}
              className="fa-regular fa-angle-left fa-lg color-868F98"
            ></i>
            <span className="pe-5"></span>
            <i
              onClick={next}
              className="fa-regular fa-angle-right fa-lg color-868F98"
            ></i>
          </div>
        </div>
      </Card>
    </div>
  );
};
