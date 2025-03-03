import React, { useEffect, useRef, useState } from "react";
import { Card, Carousel } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import TestimonialCarouselStyle from "../../stylesheets/TestimonialCarouselStyle.module.css";
import rightArrow from "../../assests/images/right-arrow.png";
import leftArrow from "../../assests/images/left-arrow.png";

const data = [
  {
    id: 1,
    text: "The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody Else Up.",
    writter: "Mercy W.",
  },
  {
    id: 2,
    text: "The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody Else Up.",
    writter: "Mercy W.",
  },
  {
    id: 3,
    text: "The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody Else Up.",
    writter: "Mercy W.",
  },
  {
    id: 4,
    text: "The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody Else Up.",
    writter: "Mercy W.",
  },
  {
    id: 5,
    text: "The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody Else Up.",
    writter: "Mercy W.",
  },
  {
    id: 6,
    text: "The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody Else Up.",
    writter: "Mercy W.",
  },
  {
    id: 7,
    text: "The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody Else Up.",
    writter: "Mercy W.",
  },
];

export const TestimonialCarousel = () => {
  const settings = {
    className: "center",
    centerMode: true,
    infinite: true,
    centerPadding: "12%",
    slidesToShow: 3,
    speed: 500,
    responsive: [
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
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

  useEffect(() => {
    console.log(document.getElementsByClassName("slick-arrow"));
    const element = document.getElementsByClassName("slick-arrow");
    const elementsArray = [...element]; // Convert to array

    elementsArray.forEach((element) => {
      console.log(element); // Do something with each element
      element.style.display = "none";
    });
  });

  return (
    <div>
      <Card className={`p-2 ${TestimonialCarouselStyle.main_card}`}>
        <span className=" fs-4 mt-2 mb-4 ps-5 text-start color-#0053A5">
          Testimonials
        </span>
        <div className="slider-container">
          <Slider
            {...settings}
            ref={(slider) => {
              containerRef = slider;
            }}
          >
            {data.map((item, index) => (
              <Card
                className={`${TestimonialCarouselStyle.flex_card} ${
                  topElement == index &&
                  TestimonialCarouselStyle.curr_slider_card
                } ${
                  prevElement == index &&
                  TestimonialCarouselStyle.prev_slider_card
                } ${
                  nextElement == index &&
                  TestimonialCarouselStyle.next_slider_card
                } ${
                  nextElement !== index &&
                  topElement !== index &&
                  prevElement !== index &&
                  TestimonialCarouselStyle.slider_card
                } border border-0`}
                key={index}
              >
                <Card.Body
                  className={`m-3 p-0 ${
                    topElement == index
                      ? TestimonialCarouselStyle.active_slider_card
                      : TestimonialCarouselStyle.slider_card
                  }`}
                >
                  <p className="m-0 text-start p-4">
                    <p className="m-0 p-0 mb-2">
                      <i
                        className={`fa-solid fa-quote-left ${
                          topElement == index
                            ? "fa-2xl color-0053A5"
                            : "fa-xl color-FFFFFF"
                        }`}
                      ></i>
                    </p>
                    <div
                      className={`mb-2 ${
                        topElement == index ? "color-002569" : "color-FFFFFF"
                      }`}
                    >
                      {item.text}
                    </div>
                    <p
                      className={`m-0 p-0 ${
                        topElement == index ? "color-000000" : "color-FFFFFF"
                      }`}
                    >
                      {item.writter}
                    </p>
                  </p>
                </Card.Body>
              </Card>
            ))}
          </Slider>
          <div className="text-center">
            <img
              src={leftArrow}
              alt=""
              onClick={previous}
              className="pointer"
            />
            <span className="pe-5"></span>
            <img src={rightArrow} alt="" onClick={next} className="pointer" />
          </div>
        </div>
        <div className="mt-5">
          <BackToLog />
        </div>
      </Card>
    </div>
  );
};
