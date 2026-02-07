# Tech Choices
Basic outline of what'll be used, subject to refinement of course. Once things are more cemented things may be captured in an ADR.

# Backend
- C# of course
- Asp .Net core naturally
- As little additional libraries where possible
	- Which isn't too hard, C#/ASP is basically almost everything out of the box
	- But it's not a strict "no libraries", but a strong, "not invented here so lets reinvent what we need" accepting the trade offs we'll obviously have
		- Structure of code will be in a way where we can swap out failed implementations with libraries that have been identified as much better
		- If a library does something we need, see if we can do the minimum of what we need
	- Exceptions being anything around JSON (it'll be `System.Text`), OIDC if we ever do it, and anything that touches any level of serious security
		- I ain't fucking with that shit when it comes to something that could be hosted publicly
# Database
- Thinking Postgres and [MartenDB](https://martendb.io/introduction.html) 
	- I'm already writing tests against Sqlite in a document db style and was already thinking towards postgres anyway
# Frontend
- Vue
	- Latest, but obviously keeping it (and any other packages), up to date constantly
- Packages will be...lol, it's frontend, but kept as minimal as possible
	- Vue comes with a lot of things out of the box, we'll make our own components
	- Axios for API calls just because I don't really want to implement that shit
	- [Pinia Colada](https://pinia-colada.esm.dev/) for wrapping around raw Axios
		- Maybe something in tanstack but _something_ about that is giving me future rug pulling vibes to me
			- Tanstack would only be TanQuery
				- But Pinia is more native to vue
		- Works better with vue developer tools anyway, natively supported in the UI it comes with
	- [VueUse](https://vueuse.org/) is also a good contender for the sacred package.json
		- Has some nice things I've written a few times like [createInjectionState](https://vueuse.org/shared/createInjectionState/) for some state sharing between components, or [useObjectUrl](https://vueuse.org/core/useObjectUrl/) for image upload to blobs for database storage/transfer to the backend
		- Then there's [useVModel](https://vueuse.org/core/useVModel/) for the boiler plate of `v-model` but even by their own docs they recommend Vue's `defineModel` which now does this natively so there's a bit less value in VueUse and the things I did use can just be written once by us anyway
	- [PrimeVue](https://primevue.org/datepicker/) component library
		- 50/50 on this one, I only really use it for the date picker if I do
		- But I also 100% have no qualms with using this for our input components to cut down on rolling our own
			- But that also feels like we're giving up a chance to be creative and learn our own way
			- Plus the components like input or range aren't that difficult, so it'd only be for the date picker
		- Using a component library does mean anything custom we want is built around any assumptions and implementation quirks they designed around, rather than just building on top of base HTML inputs
	- [Vue DatePicker](https://vue3datepicker.com/) would use this as it's usually the only component I don't really want to implement myself
- Will be statically built and distributed with the backend as an all in one package
	- No separate frontend backend host bullshit fuck that noise lmao
		- "Wow I sure am glad I have to host 2 halves of the 1 application individually and deal with punching my genitals with CORS because then I can update just the frontend independently from the backend", said no one except that one person we _all_ know
		- This is a small project lmao, shit just needs to work with as little friction as possible
# Containers
- Yeah this'll be mostly done in containers just because I don't want people to have to install shit locally for the database and runtimes etc
	- I would make this is as a pure desktop app if I wanted but being able to manage inventory remotely even on a local network wouldn't be easy with that design
- podman, docker, whatever, don't really care
	- Just going to create some docker files that builds the image for the app itself
	- Don't really care about putting that image up on docker hub, just going to supply the docker file so people can run 1 extra command locally before running the compose file
		- Bit jank and more steps but whatever, fuck centralising shit like this to docker hub that's uncontrolled from the actual source