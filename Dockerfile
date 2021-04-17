#
# docker pull ubuntu
# docker build --tag myubuntu  .
# 


FROM ubuntu

# Set a default shell.

SHELL ["/bin/bash", "-c"]


ARG DEBIAN_FRONTEND=noninteractive
ENV TZ="America/Salt Lake City"


RUN apt-get -y update && apt-get -y install \
  git \
  apt-utils \
  emacs-nox \
  g++ \
  cmake \
  valgrind \
  gdb \ 
  libboost-all-dev
  
  
RUN git clone https://github.com/miloyip/rapidjson.git

RUN cp -r rapidjson/include/rapidjson/ /usr/local/include

COPY . /FinalProject