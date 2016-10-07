package com.purlieus.purlieus.adapters;

import android.content.Context;
import android.content.Intent;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.purlieus.purlieus.R;
import com.purlieus.purlieus.models.BD_Seek;

import java.util.List;

import static android.R.attr.id;

/**
 * Created by Shaurya on 06-Oct-16.
 */

public class SeekAdapter extends RecyclerView.Adapter<SeekAdapter.UserViewHolder> {

    private List<BD_Seek> list;
    private LayoutInflater inflater;
    private Context context;


    public SeekAdapter(Context context, List<BD_Seek> list) {
        inflater = LayoutInflater.from(context);
        this.list = list;
        this.context = context;
    }

    @Override
    public UserViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

        View view;

        /*if(viewType == 1)
            view = inflater.inflate(R.layout.item_seeker_urgent, parent, false);
        else*/
            view = inflater.inflate(R.layout.item_seeker, parent, false);

        UserViewHolder holder = new UserViewHolder(view);

        return holder;
    }

    @Override
    public void onBindViewHolder(UserViewHolder holder, int position) {

        BD_Seek model = list.get(position);
        holder.name.setText(model.getName());
        holder.bloodGroup.setText(model.getBloodGroup());
        holder.age.setText(Integer.toString(model.getAge()));
        holder.phoneNum.setText(model.getContactNumber());
        holder.sex.setText(model.getSex());

    }

    /*@Override
    public int getItemViewType(int position) {
        if (list.get(position).isUrgent()) {
            return 1;
        } else {
            return 0;
        }
    }*/

    @Override
    public int getItemCount() {
        return list.size();
    }

    class UserViewHolder extends RecyclerView.ViewHolder implements View.OnClickListener {

        TextView name, bloodGroup, phoneNum, sex, age;

        public UserViewHolder(View itemView) {
            super(itemView);

            itemView.setOnClickListener(this);
            name = (TextView) itemView.findViewById(R.id.name_item);
            bloodGroup = (TextView) itemView.findViewById(R.id.blood_group_type_item);
            phoneNum = (TextView) itemView.findViewById(R.id.contact_num_item);
            sex = (TextView) itemView.findViewById(R.id.sex_type_item);
            age = (TextView) itemView.findViewById(R.id.age_value_item);

        }


        @Override
        public void onClick(View view) {

        }
    }
}
