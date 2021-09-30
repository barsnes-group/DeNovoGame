import csv
import matplotlib.pyplot as plt

amino_acids = {
    "N": 114,
    "V": 99
}

def read_file(file_in):
    coord = []
    with open(file_in, 'r') as csvfile:
        reader = csv.reader(csvfile, delimiter=' ')
        for row in reader:
            tuple = (float(row[0]), float(row[1]))
            coord.append(tuple)
    return coord


def write_to_file(filename_out):
    with open(filename_out, 'w') as csvfile:
        writer = csv.writer(csvfile)

        for w in range(len(mz)):
            writer.writerow([mz[w], int_graph[w]])
        csvfile.close()


def filter_amino_acid(coordinates):
    valid_coor = []
    for i, (first_valueX, first_valueY) in enumerate(coordinates):
        for(second_valueX, second_valueY) in coordinates[i+1:]:
            distance = second_valueX - first_valueX
            if int(distance) in amino_acids.values():
                valid_coor.append((first_valueX, first_valueY))
                valid_coor.append((second_valueX, second_valueY))
                #print(distance, "x1 ", first_valueX, "x2: ", second_valueX  )

    # remove duplicates
    valid_coor = list(dict.fromkeys(valid_coor))
    return valid_coor


def normalize_data():
    pass


def pluss_minus():
    pass


def percentage(part, whole):
    return (part/whole*100)


def filter_on_percentage(coordinates, threshold_percentage):
    valid_coor = []
    y_coord = get_y_coor(coordinates)
    print(y_coord)

    for (x, y) in coordinates:
        if percentage(y, max(y_coord)) >= threshold_percentage:
            valid_coor.append((x, y))
    return valid_coor


def get_x_coor(coordinates):
    return [x[0] for x in coordinates]


def get_y_coor(coordinates):
    return [y[1] for y in coordinates]


def main():
    coordinates = read_file('Assets/Scripts/selected_spectra.csv')
    filtered_on_percentage = filter_on_percentage(coordinates, 10)
    
    filtered_on_amino_acids_and_percentage = filter_amino_acid(filtered_on_percentage)
    

    print(f"Number of peaks before filtering: {len(coordinates)} \nNumber of peaks after filtering: {len(filtered_on_amino_acids_and_percentage)}")

    # plot graph
    plt.bar(get_x_coor(coordinates), get_y_coor(coordinates))
    plt.xlabel('m/z')
    plt.ylabel('int')
    plt.legend()
    plt.show()

    plt.bar(get_x_coor(filtered_on_amino_acids_and_percentage), get_y_coor(filtered_on_amino_acids_and_percentage))
    plt.xlabel('m/z')
    plt.ylabel('int')
    plt.legend()
    plt.show()


main()
