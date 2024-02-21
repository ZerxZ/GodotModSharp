extends Node


# Called when the node enters the scene tree for the first time.
func _ready():
	var mod=load("res://addons/GodotModSharp/ModManager.cs")
	print(mod.source_code)
	add_child(mod.new())



