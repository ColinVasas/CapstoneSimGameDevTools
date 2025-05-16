# CapstoneSimGameDevTools

# Semiconductor Cleanroom Training Simulation

![alt text](https://github.com/ColinVasas/CapstoneSimGameDevTools/blob/yellowRoomMerge/images/semi.jpg)
 
Figure 1; Screenshot of our modeled wetbench

A collaborative VR simulation Senior Capstone project crafted by Oregon State University students for hands-on semiconductor training–without the cost, danger, or waste. 

### What is a Cleanroom?

A cleanroom is a sterile environment where semiconductors and other products can be safely made, with minimal risk of complications.

Even the smallest speck of dust can cause massive damage to a semiconductor, so a clean and sterile environment is needed to safely create microchips and other products.

# Why a Semiconductor Cleanroom? 

### Not Enough Semiconductors Are Being Produced 

In general, the number of semiconductor cleanrooms is low for the amount of computer chips needed to be produced. This inequality was caused by the increase in demand for compute capabilities for things like AI and other emerging technologies. Additionally, cleanrooms are very expensive to build due to the infrastructure needed to make the space air tight. 

This has created the need for more qualified cleanroom personnel than ever before to assure the cleanrooms are producing enough high-quality semiconductors and no accidental damages occur. 

### Training is Time Consuming and Costly

The typical training makes trainees conduct hours of on-the-job training in these labs, which need to be supervised. This affects the supervisor’s schedule and may cause time conflicts within the cleanroom itself. There is only so much time in the day, which forces companies to choose whether training or optimal production time is most important. 

The traditional training bottleneck has become increasingly problematic with new graduate students and new technicians facing long wait periods. 

## What our project does

![alt text](https://github.com/ColinVasas/CapstoneSimGameDevTools/blob/yellowRoomMerge/images/semi2.png)
Figure 2; Close up of the spin coater with a wafer being placed

To address the growing demand, we are developing a Virtual Reality cleanroom training simulation. Our simulation will provide a risk-free training environment to educate trainees, students, and other interested parties on the general cleanroom production process. 

We created a virtual semiconductor cleanroom space with the intent to:

* help users learn the procedures and behaviors you would see in a clean room  
* train others virtually on how to operate the tools presented in a cleanroom at a high level  
* guide users through the three major processes of creating a semiconductor: the photolithography, etching, and deposition stages  
* be usable as an educational simulation/game for college students

With the use of Virtual Reality, we are able to go into way more depth with how the clean room environment is meant to be facilitated. This project could be the first of many virtual reality training programs that save companies money by training new employees without having to risk damages that could occur on the job.

### Similar products

Though you can easily find some products that are designed to help with the actual production of semiconductors, like [NVIDIA’s Industrial Facility Digital Twins](https://www.nvidia.com/en-us/use-cases/industrial-facility-digital-twins/?ncid=pa-srch-goog-769746&_bt=747914857804&_bk=manufacturing%20simulation&_bm=b&_bn=g&_bg=184182243051&gad_source=1&gad_campaignid=22476983236&gbraid=0AAAAAD4XAoEjjXuEV2X3Dx02SLWbkvMpY&gclid=Cj0KCQjwoZbBBhDCARIsAOqMEZU-WVzmEtoDPFQeRaspEsKonk2TAq7xoad8qv7GSfWgAFLWddq9X-kaAk1oEALw_wcB), there aren’t many products that are open to the public and mainly used for the educational process. 

# Core Project Features

![alt text](https://github.com/ColinVasas/CapstoneSimGameDevTools/blob/yellowRoomMerge/images/semi3.jpg)
Figure 3; image showing the equip feature with UI

Our project is a first-person virtual reality training simulator designed to guide users through a tutorial that demonstrates the main components of the semiconductor fabrication process from station to station. Because every clean room is different and professional clean rooms are not open to the public, our simulation does not comprehensively cover all clean room procedures. However, the tasks it does include are generally applicable to clean rooms in general.

The simulator walks users through each step, allowing them to pick up, move, and manipulate objects and equipment to get a feel for the space while learning the protocols.

### Technologies used

The main software development programs used were Unity for the environment and Visual Studio for creating the C\# scripts.

Blender was also used for modeling the machines and various objects in space. All models made will be included in this Github repository. 

![alt text](https://github.com/ColinVasas/CapstoneSimGameDevTools/blob/yellowRoomMerge/images/semi4.jpg)
Figure 4; Chart of the technologies used in the screenshot.

### General Architecture

![alt text](https://github.com/ColinVasas/CapstoneSimGameDevTools/blob/yellowRoomMerge/images/semi5.jpg)
Figure 5; Flowchart of the general flow of code through the production process.

In Figure 5, above, you can see how coded scripts are implemented into the project. They start in Visual Studio, or any other C\# compatible coding application, and are then implemented in Unity by attaching them to an object. That object is then usable in the Unity scene for the desired purpose.  
 

## The Development Process/Project Status 

Initial Planning and Research Phase:

* Toured an Oregon State cleanroom and observed an actual layout, workflow and equipment arrangement  
* Received expert guidance from clean room technician who provided specialized knowledge

Foundation Development:

* Created models in Blender to use in the simulation  
* Coded basic character movement and controls in Virtual Reality  
* Implemented control interaction for object grabbing, manipulation, and movement using the Unity XR toolkit  
* Established various modular features and utility systems to support the project’s development

Core Cleanroom Implementation:  
The team split into further subteams to develop the cleanroom stations and machines.

Gowning Room:

* This scene requires the user to “equip” ordered items  
* WASD to move  
* Left mouse button to pick Items up, and left mouse button again to drop  
* Hold the “R” Key and move the mouse to rotate Object.

Yellow Room:

* Press “E” to interact  
* Triggers zones, executes a function when entering, inside or leaving the trigger zone  
* Physical Vapor Deposition (PVD) system renders deposited material on the wafer using layers of Unity materials

Wet Etching:

* Teaching the process of identifying chemicals, mixing them, and submerging a microchip for the right time while avoiding danger.

### Challenges We Have Encountered in Development

One challenge we faced stemmed from having such a large team. Communication is difficult with ten members, but we were able to split the team into sub teams and have most communication between teams conducted through team leaders. 

Another challenge we came across was making sure that the VR was working correctly. There were a couple times where testing revealed that something in development broke VR functionality so we had to be extra sure that it is in a functional state.

We also had an initial difficulty figuring out the direction we wanted to take this project, or at least our year worth of work on it. We were given an idea and were let loose to figure it out on our own. We were able to figure out where to start when we talked with some other professionals and professors, thankfully.  

The biggest challenge we faced was using new technologies that we weren't familiar with. The learning curve that the majority of us faced learning Unity and Blender was thankfully not very difficult, but was time consuming. This paired with not having a ton of time because of other classes gave us some issues when trying to compile our files together. However, we were all able to help each other out to speed up the process.

### What’s Next?

Because of the nature of this project, the exact next steps are truly up to the developers. That said, the general direction is to keep adding functional machines to the space to make the space more and more usable. 

Eventually, if this project was able to educate the user on the entire process of making a semiconductor, that would be the long term goal. The entire process being, from getting the silicon wafers, to selling the finished product. 

The planned next steps, or what our group (named at the bottom of the document) would have worked on next, would be to refine or add some more intermediate steps to the parts of the process we have done so far. For example, adding a dryer for the wet etching process, including more safety tips, or adding a cleanliness factor to where if you drop something it either gets dirty or breaks. 

# Access and Usage

### Installation Requirements

We recommend having system specifications of at least:

* 8GB RAM  
* Intel I5 or newer  
* Intel Iris Plus Integrated Graphics, Nvidia 1060, or newer

‘The better the graphics card, the better the performance’ applies here. General specs based on the lowest end computer used for development. Though it may be able to run on a lower end computer, we cannot guarantee a smooth experience. 

### To download the project and run using Unity Hub:

You will need to download and install Unity and this Project.   
![alt text](https://github.com/ColinVasas/CapstoneSimGameDevTools/blob/yellowRoomMerge/images/semi6.jpg) 
Figure 6; screenshot of the top of the project 

Before downloading, make sure that the main branch is selected at the top of this project.

**The friendliest way to download this project is to click on the “Code” button at the top of this project, then select the download .zip option at the bottom of the drop down.** The download will then start automatically and be saved in your computer’s default downloads folder. 

Once the download is complete, extract all the files to a location you will remember.

Additionally, if you know how to clone repositories using [Git](https://git-scm.com/), that option is available as well.

### To run the project: 

![alt text](https://github.com/ColinVasas/CapstoneSimGameDevTools/blob/yellowRoomMerge/images/semi7.jpg) 
Figure 7; screenshot of Unity Hub’s projects tab with semiSim project

Open Unity Hub; If it is your first time trying to run the project, you will need to select “Add” towards the top. Then “Add project from disk”, navigate to where you extracted the .zip, and select the “semiSim” folder. 

Then, back in Unity Hub, once you make sure your Unity [editor versions](https://unity.com/download) are compatible with the project, run the project editor by clicking on the newly added “semiSim” unity project. 

Once it opens, select the “Scenes” folder from the lower left side list. If you are running it on a desktop, select the “gowningRoomDesk” scene. If on VR, select the “gowningRoomVr” scene.

Then at the top of the application, press the play button. 

# Team Members / Contact Information

If you have any questions about the project or code, please feel free to contact us at any of the emails below.

Arno Porter (arnoporter@gmail.com) \- Subteam Leader, Blender, & Frontend Developer

Caden Garret (garrecad@oregonstate.edu) \- Unity and Backend Programmer

Caleb Struve (calebstruve6@gmail.com) \- Backend and Frontend Programmer

Colin Vasas (colinvasas@gmail.com) \- Unity and Backend Programmer

Domanik Logan (domaniklogan2002@gmail.com) \- Unity and Backend Programmer

Dylan Miner (dylanm1n3r@gmail.com) \- Blender and Frontend Developer

Garrett Mcmillan (garrettcmcmillan@gmail.com) \- Blender Leader/Modeler

Jonathan Fairgrieve (fairgrievecs@gmail.com) \- Unity and Backend Programmer

Karson Hartman (hartman.kah@gmail.com) \- Blender and Frontend Developer

Talal Alkhalaf (talala.alkhalaf@gmail.com) \- Unity and Backend Leader

