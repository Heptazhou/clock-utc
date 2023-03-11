# Copyright (C) 2023 Heptazhou <zhou@0h7z.com>
#
# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU Affero General Public License as
# published by the Free Software Foundation, either version 3 of the
# License, or (at your option) any later version.
#
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU Affero General Public License for more details.
#
# You should have received a copy of the GNU Affero General Public License
# along with this program.  If not, see <https://www.gnu.org/licenses/>.

const a = `-m0=lzma -md1024m -mfb273 -mmt -ms -mx9 -stl -xr!\*.pdb`
const c = "Release"
const f = "net7.0-windows"
const n = "Clock"

try
	cd(@__DIR__)
	rm("$n.7z", force = true)
	rm("$n.sln", force = true)
	rm("$n/obj/", force = true, recursive = true)
	rm("$n/bin/", force = true, recursive = true)
	run(`dotnet --info`)
	run(`dotnet new sln -n $n`)
	run(`dotnet sln add $(n)`)
	run(`dotnet build -c $c`)
	mv("$n/bin/$c/$f/", "$n/bin/$c/$n/")
	run(`7z a $a $n.7z ./$n/bin/$c/$n/`)
	rm("$n/bin/$c/$n", recursive = true)
	rm("$n.sln")
catch e
	@info e
end
isempty(ARGS) && readline()

